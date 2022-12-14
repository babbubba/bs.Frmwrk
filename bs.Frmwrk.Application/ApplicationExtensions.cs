﻿using bs.Data;
using bs.Data.Interfaces;
using bs.Frmwrk.Application.Filters;
using bs.Frmwrk.Application.Models.Configuration;
using bs.Frmwrk.Auth.Services;
using bs.Frmwrk.Base.Exceptions;
using bs.Frmwrk.Core.Globals.Config;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Base;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Locale.Providers;
using bs.Frmwrk.Locale.Services;
using bs.Frmwrk.Mailing.Models;
using bs.Frmwrk.Security.Models;
using bs.Frmwrk.Security.Services;
using bs.Frmwrk.Shared;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace bs.Frmwrk.Application
{
    public static class ApplicationExtensions
    {
        private static ICoreSettings? coreSettings;
        private static IEmailSettings? emailSettings;
        private static IFileSystemSettings? fileSystemSettings;
        private static ILoggingSettings? loggingSettings;
        private static ISecuritySettings? securitySettings;
        private static IDbContext? dbContext;

        public static void InitFrmwrk(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.SetCustomConfigFile();
            builder.InitConfiguration();

            builder.InitLogging();
            builder.SetLocalization();
            builder.InitORM();
            builder.SetAuthorization();
            builder.SetFileSystem();
            builder.LoadExternalDll();
            builder.RegisterRepositories();
            builder.RegisterServices();
            builder.SetCors();
            builder.SetControllers();
            builder.InitSwagger();
        }

        internal static void InitConfiguration(this WebApplicationBuilder builder)
        {
            loggingSettings = new LoggingSettings();
            builder.Configuration.GetSection("Logging").Bind(loggingSettings);
            builder.Services.AddSingleton(loggingSettings);

            dbContext = new DbContext();
            builder.Configuration.GetSection("Database").Bind(dbContext);

            coreSettings = new CoreSettings();
            builder.Configuration.GetSection("Core").Bind(coreSettings);
            builder.Services.AddSingleton(coreSettings);

            securitySettings = new SecuritySettings();
            builder.Configuration.GetSection("Security").Bind(securitySettings);
            builder.Services.AddSingleton(securitySettings);

            fileSystemSettings = new FileSystemSettings();
            builder.Configuration.GetSection("FileSystem").Bind(fileSystemSettings);
            builder.Services.AddSingleton(fileSystemSettings);

            emailSettings = new EmailSettings();
            builder.Configuration.GetSection("Mailing").Bind(emailSettings);
            builder.Services.AddSingleton(emailSettings);
        }

        internal static void InitLogging(this WebApplicationBuilder builder)
        {
            if (loggingSettings is null)
            {
                throw new BsException(2212141610, "Invalid configuration section 'Logging'");
            }

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

        internal static void InitORM(this WebApplicationBuilder builder)
        {
            if(dbContext is null)
            {
                throw new BsException(2212141610, "Invalid configuration section 'Database'");
            }       

            // Set the ORM auto schema update
            dbContext.Update = true;

            // Register the ORM
            builder.Services.AddBsData(dbContext);
        }

        internal static void LoadExternalDll(this WebApplicationBuilder builder)
        {
            var result = new Dictionary<string, IApiResponseViewModel>();
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

        internal static void RegisterRepositories(this WebApplicationBuilder builder)
        {
            var result = new Dictionary<string, ApiResponseViewModel>();
            var repositories = typeof(IRepository).GetTypesFromInterface();

            foreach (var repository in repositories)
            {
                if (repository is null)
                {
                    continue;
                }

                try
                {
                    var repositoryInterfaces = repository.GetInterfacesOf(new System.Type[] { typeof(IRepository) });
                    if (repositoryInterfaces == null)
                    {
                        result.Add(repository.FullName ?? repository.Name, new ApiResponseViewModel(false, $"No interfaces found for repository: '{repository.FullName ?? repository.Name}'"));
                        continue;
                    }
                    foreach (var repositoryInterface in repositoryInterfaces)
                    {
                        if (repositoryInterface is not null)
                        {
                            builder.Services.AddScoped(repositoryInterface, repository);
                            result.Add((repository.FullName ?? repository.Name) + $" [{repositoryInterface.FullName ?? repositoryInterface.Name}]", new ApiResponseViewModel());
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Add(repository.FullName ?? repository.Name, new ApiResponseViewModel(false, ex.Message));
                }
            }
            //TODO: Log dei risultati della registrazione dei repositories
        }

        internal static void RegisterServices(this WebApplicationBuilder builder)
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
                    var serviceInterfaces = service.GetInterfacesOf(new System.Type[] { typeof(IBsService)/*, typeof(IInitializableService) */});
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

        internal static void SetAuthorization(this WebApplicationBuilder builder)
        {
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

            // Setting authentication using JWT Token in request header
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = securitySettings?.ValidateIssuer ?? false,
                        ValidateAudience = securitySettings?.ValidateAudience ?? false,
                        ValidAudience = securitySettings?.ValidTokenAudience,
                        ValidIssuer = securitySettings?.ValidTokenIssuer,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.Secret))
                    };
                    //Added to autenticate Signal R(token is in the query part of the url)
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments(UrlsConfigurations.SignalRHubPrefix))
                            {
                                // If the request is addressed to any hubs get the token from the access_token query parameter
                                var accessToken = context.Request.Query["access_token"];
                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    // Read the token out of the query string
                                    context.Token = accessToken;
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        internal static void SetCors(this WebApplicationBuilder builder)
        {
            var conf = new CorsPolicyBuilder();
            conf.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();

            if (coreSettings?.FrontendOrigins is not null && coreSettings.FrontendOrigins.Length > 0)
            {
                conf.WithOrigins(coreSettings.FrontendOrigins);
            }

            var corsPolicyConfig = conf.Build();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(CorsConfigurations.CorsPolicyName, corsPolicyConfig);
            });
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

        internal static void SetFileSystem(this WebApplicationBuilder builder)
        {
            // File upload limits
            builder.Services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = fileSystemSettings?.ValueLengthLimitMb * 1024 * 1024 ?? int.MaxValue;
                o.MultipartBodyLengthLimit = fileSystemSettings?.MultipartBodyLengthLimitMb * 1024 * 1024 ?? int.MaxValue;
                o.MemoryBufferThreshold = fileSystemSettings?.MemoryBufferThresholdMb * 1024 * 1024 ?? int.MaxValue;
            });
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

        internal static void SetControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(conf =>
            {
                conf.AllowEmptyInputInBodyModelBinding = true;
            });
        }

        internal static void InitSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(conf =>
            {
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**. To get the token call **/account/authenticate** passing **username** and **password**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", 
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                conf.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                conf.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {securityScheme, new string[] { }}
                    });

                // Documentation
                conf.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = coreSettings?.AppTitle ?? "Application",
                    Version = "v1",
                    Description = "API references for " + coreSettings?.AppTitle ?? "Application",
                    Contact = new OpenApiContact
                    {
                        Name = coreSettings?.AppCompany ?? "Company",
                        Email = string.Empty,
                        Url = coreSettings?.CompanyWebSite is not null ? new Uri("http://www.italcom.it") : null,
                    }
                });

                // Include all projects xml documentations
                var xmlPaths = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xmlPath in xmlPaths)
                {
                    conf.IncludeXmlComments(xmlPath);
                    conf.SchemaFilter<SwaggerEnumSchemaFilter>();
                }
            });
        }
    }
}