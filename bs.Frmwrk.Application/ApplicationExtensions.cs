using bs.Data;
using bs.Data.Interfaces;
using bs.Frmwrk.Application.Models.Configuration;
using bs.Frmwrk.Auth.Services;
using bs.Frmwrk.Base.Exceptions;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Locale.Providers;
using bs.Frmwrk.Locale.Services;
using bs.Frmwrk.Security.Models;
using bs.Frmwrk.Security.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using bs.Frmwrk.Shared;
using bs.Frmwrk.Mailing.Models;
using Microsoft.AspNetCore.Http.Features;
using bs.Frmwrk.Core.ViewModels.Api;
using MySqlX.XDevAPI.Common;
using bs.Frmwrk.Core.Services.Base;

namespace bs.Frmwrk.Application
{
    public static class ApplicationExtensions
    {
        public static void InitFrmwrk(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.SetCustomConfigFile();
            builder.InitLogging();
            builder.SetLocalization();
            builder.InitORM();
            builder.SetAuthorization();
        }

        internal static void SetCustomConfigFile(this WebApplicationBuilder builder)
        {
            var configfilePath = Path.Combine(builder.Environment.ContentRootPath, $"configuration.{builder.Environment.EnvironmentName}.json");
            if (!File.Exists(configfilePath))
            {
                // Cannot init application because config file doesnt exist
                throw new Exception($"No valid configuration file found at path: {configfilePath}");
            }

            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
            builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();
        }

        internal static void InitLogging(this WebApplicationBuilder builder)
        {
            ILoggingSettings loggingSettings = new LoggingSettings();
            builder.Configuration.GetSection("Logging").Bind(loggingSettings);
            builder.Services.AddSingleton(loggingSettings);

            LoggingLevelSwitch loggingLevel;
            if (loggingSettings.Debug is not null && loggingSettings.Debug == true)
            {
                loggingLevel = new LoggingLevelSwitch(LogEventLevel.Debug);
            }
            else
            {
                loggingLevel = new LoggingLevelSwitch(LogEventLevel.Warning);
            }

            if (Log.Logger != null) Log.CloseAndFlush();

            string logFile = Path.Combine(loggingSettings.Path ?? builder.Environment.ContentRootPath, loggingSettings.LogFileName ?? "application.log");

            var loggerConfiguration = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .WriteTo.File(logFile, rollingInterval: RollingInterval.Day,
                                 retainedFileCountLimit: loggingSettings.RetentionDays ?? 15,
                                 fileSizeLimitBytes: loggingSettings.LogFileLimitInBytes ?? 4194304, // (4 MB)
                                 rollOnFileSizeLimit: true,
                                 outputTemplate: loggingSettings.Template ?? "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj} {NewLine}{Exception}",
                                 shared: true)
                            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                            .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.Extensions.Localization.ResourceManagerStringLocalizer", LogEventLevel.Warning)
                            .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                            .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.Extensions.Http.DefaultHttpClientFactory", LogEventLevel.Warning)
                            .MinimumLevel.ControlledBy(loggingLevel);

            if (!string.IsNullOrWhiteSpace(loggingSettings.SeqEndpoint))
            {
                var assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;

                loggerConfiguration
                           .Enrich.WithProperty("ApplicationName", loggingSettings.ApplicationName ?? "*")
                           .Enrich.WithProperty("AssemblyVersion", productVersion ?? "*")
                          .WriteTo.Seq(loggingSettings.SeqEndpoint);
            }

            //loggerConfiguration.WriteTo.Udp(udpHost, udpPort, AddressFamily.InterNetwork, formatter: new Serilog.Sinks.Udp.TextFormatters.Log4jTextFormatter())

            if (loggingSettings.Debug is not null && loggingSettings.Debug == true)
            {
                Log.Debug($"Service is running in debug mode. You can disable this in the configuration file setting the 'EnableDebug' property in the 'AppConfiguration' section to false.");
            }
            //Log.Logger = loggerConfiguration.CreateLogger();

            //builder.Logging.ClearProviders();
            //builder.Logging.AddSerilog(logger);
            //builder.Host.UseSerilog();

            builder.Host.UseSerilog((ctx, lc) => lc = loggerConfiguration);
        }

        internal static void SetLocalization(this WebApplicationBuilder builder)
        {
            //TODO: Crea parametri impostazioni per lingue ammesse e lingua di default
            builder.Services.AddScoped<ITranslateService, TranslateService>();

            builder.Services.AddLocalization(); // we dont set the path so Localization assume that resource file is in the same path as 'Languages' shared class (Italcom.TodoStore.Infrastructure project in the Localization folder)
            builder.Services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("it-IT"),
                        new CultureInfo("en-US")
                    };
                    options.DefaultRequestCulture = new RequestCulture("it-IT", "it-IT");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders = new[] { new AcceptLanguageHeaderDataRequestCultureProvider() };
                });
        }

        internal static void SetAuthorization(this WebApplicationBuilder builder)
        {
            ISecuritySettings securitySettings = new SecuritySettings();
            builder.Configuration.GetSection("Security").Bind(securitySettings);
            builder.Services.AddSingleton(securitySettings);

            var authRepository = typeof(IAuthRepository).GetTypeFromInterface();
            if (authRepository == null)
            {
                throw new BsException(2212111515, "Cannot find a valid implementation of the 'IAuthRepository' interface.");
            }

            var securityRepository = typeof(ISecurityRepository).GetTypeFromInterface(); ;
            if (securityRepository == null)
            {
                throw new BsException(2212111516, "Cannot find a valid implementation of the 'ISecurityRepository' interface.");
            }

            builder.Services.AddScoped(typeof(IAuthRepository), authRepository);
            builder.Services.AddScoped(typeof(ISecurityRepository), securityRepository);
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ISecurityService, SecurityService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }

        internal static void InitORM(this WebApplicationBuilder builder)
        {
            IDbContext dbContext = new DbContext();
            builder.Configuration.GetSection("Database").Bind(dbContext);

            // Set the ORM auto schema update
            dbContext.Update = true;

            // Register the ORM
            builder.Services.AddBsData(dbContext);
        }

        public static void SetMailing(this WebApplicationBuilder builder)
        {
            IEmailSettings emailSettings = new EmailSettingsModel();
            builder.Configuration.GetSection("Mailing").Bind(emailSettings);
            builder.Services.AddSingleton(emailSettings);
        }

        public static void SetFileSystem(this WebApplicationBuilder builder)
        {
            IFileSystemSettings fileSystemSettings = new FileSystemSettingsModel();
            builder.Configuration.GetSection("FileSystem").Bind(fileSystemSettings);
            builder.Services.AddSingleton(fileSystemSettings);

            // File upload limits
            builder.Services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = fileSystemSettings.ValueLengthLimitMb * 1024 * 1024 ?? int.MaxValue;
                o.MultipartBodyLengthLimit = fileSystemSettings.MultipartBodyLengthLimitMb * 1024 * 1024 ?? int.MaxValue;
                o.MemoryBufferThreshold = fileSystemSettings.MemoryBufferThresholdMb * 1024 * 1024 ?? int.MaxValue;
            });
        }

        public static void LoadExternalDll(this WebApplicationBuilder builder)
        {
            var result = new Dictionary<string, IApiResponseViewModel>();
            ICoreSettings coreSettings = new CoreSettingsModel();
            builder.Configuration.GetSection("Core").Bind(coreSettings);
            builder.Services.AddSingleton(coreSettings);

            var dllPaths = Directory.GetFiles(coreSettings.ExternalDllFilesRootPath ?? builder.Environment.ContentRootPath, coreSettings.ExternalDllFilesSearchPattern ?? $"*.dll", SearchOption.AllDirectories);
            foreach (var dllPath in dllPaths)
            {
                try
                {
                    Assembly.LoadFrom(dllPath);
                    result.Add(dllPath, new ApiResponseViewModel());
                }
                catch (Exception ex)
                {
                    result.Add(dllPath, new ApiResponseViewModel(false, ex.Message));
                }
            }

            //TODO: Log dei risultati dell'importazione delle librerie dinamiche

        }

        public static void RegisterRepositories(this WebApplicationBuilder builder)
        {
            var result = new Dictionary<string, ApiResponseViewModel>();
            var repositories = typeof(IRepository).GetTypesFromInterface();


            foreach (var repository in repositories)
            {
                if(repository is null)
                {
                    continue; 
                }

                try
                {
                    var repositoryInterfaces = repository.GetInterfacesOf(new Type[] { typeof(IRepository)});
                    if(repositoryInterfaces == null) {
                        result.Add(repository.FullName ?? repository.Name, new ApiResponseViewModel(false, $"No interfaces found for repository: '{repository.FullName?? repository.Name}'"));
                        continue; 
                    }
                    foreach(var repositoryInterface in repositoryInterfaces)
                    {
                        if(repositoryInterface is not null)
                        {
                            builder.Services.AddScoped(repositoryInterface, repository);
                            result.Add((repository.FullName ?? repository.Name) + $" [{repositoryInterface.FullName ?? repositoryInterface.Name}]", new ApiResponseViewModel());
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Add(repository.FullName ?? repository.Name, new ApiResponseViewModel( false,  ex.Message));
                }
            }
            //TODO: Log dei risultati della registrazione dei repositories

        }

        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            var result = new Dictionary<string, ApiResponseViewModel>();
            var services = typeof(IBsService).GetTypesFromInterface();

            foreach (var service in services)
            {
                if (service is null)
                {
                    continue;
                }

                try
                {
                    var serviceInterfaces = service.GetInterfacesOf(new Type[] { typeof(IBsService)/*, typeof(IInitializableService) */});
                    if (serviceInterfaces == null)
                    {
                        result.Add(service.FullName ?? service.Name, new ApiResponseViewModel(false, $"No interfaces found for service: '{service.FullName ?? service.Name}'"));
                        continue;
                    }
                    foreach (var serviceInterface in serviceInterfaces)
                    {
                        if (serviceInterface is not null)
                        {
                            builder.Services.AddScoped(serviceInterface, service);
                            result.Add((service.FullName ?? service.Name) + $" [{serviceInterface.FullName ?? serviceInterface.Name}]", new ApiResponseViewModel());
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Add(service.FullName ?? service.Name, new ApiResponseViewModel(false, ex.Message));
                }
            }
            //TODO: Log dei risultati della registrazione dei services
        }

    }
}