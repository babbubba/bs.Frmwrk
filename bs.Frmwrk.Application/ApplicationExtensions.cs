using bs.Frmwrk.Application.Models.Configuration;
using bs.Frmwrk.Core.Models.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;

namespace bs.Frmwrk.Application
{
    public static class ApplicationExtensions
    {
        public static void InitFrmwrk(this WebApplicationBuilder builder)
        {

        }

        public static void SetCustomConfigFile(this WebApplicationBuilder builder)
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

        public static void InitLogging(this WebApplicationBuilder builder)
        {
            ILoggingSettings loggingSettings = new LoggingSettings();
            builder.Configuration.GetSection("AppLog").Bind(loggingSettings);
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

            //Log.Logger = new LoggerConfiguration()
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


            //builder.Host.UseSerilog(Log.Logger);
        }

    }
}