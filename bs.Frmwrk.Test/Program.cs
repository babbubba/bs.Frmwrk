using bs.Frmwrk.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace bs.Frmwrk.Test
{
    public  class Program
    {


        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting up");

            //var logger = LoggerFactory.Create(config =>
            //{
            //    config.AddConsole();
            //}).CreateLogger("Program");


            var builder = WebApplication.CreateBuilder(args);
            try
            {
                builder.SetCustomConfigFile();
                builder.InitLogging();

                var app = builder.Build();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Logger.Fatal("Application start failed.", ex);
                throw;
            }
            finally
            {
                Log.Information("Shut down complete");
                Log.CloseAndFlush();
            }
            //    var logger = Log.Logger;

            //    try
            //    {
            //        logger.Debug("Reading config file ...");
            //        SetCustomConfigFile(builder);

            //        logger.Debug("Initing logging ...");
            //        InitLogging(builder);

            //        logger.Debug("Reading config security settings ...");
            //        IAppSecuritySettings appSecurity = ImportNRegisterAppSecuritySettings(builder);

            //        logger.Debug("Reading config ui settings ...");
            //        IAppUISettings appUI = ImportNRegisterAppUISettings(builder);

            //        logger.Debug("Reading config file system settings ...");
            //        IFileSystemSettings fileSystem = ImportNRegisterAppFileSystemSettings(builder);

            //        logger.Debug("Reading config email settings ...");

            //        IEmailSettings emailSettings = ImportNRegisterEmailSettings(builder);

            //        // File upload limits
            //        builder.Services.Configure<FormOptions>(o =>
            //        {
            //            o.ValueLengthLimit = fileSystem.ValueLengthLimitMb * 1024 * 1024 ?? int.MaxValue;
            //            o.MultipartBodyLengthLimit = fileSystem.MultipartBodyLengthLimitMb * 1024 * 1024 ?? int.MaxValue;
            //            o.MemoryBufferThreshold = fileSystem.MemoryBufferThresholdMb * 1024 * 1024 ?? int.MaxValue;
            //        });

            //        logger.Debug("Configuring O.R.M. ...");
            //        InitORM(builder);

            //        logger.Debug("Registering repositories and services ...");
            //        RegisterRepositoriesAndServices(builder);

            //        logger.Debug("Configuring mapping ...");
            //        RegisterMapping(builder);

            //        // Setting CORS
            //        logger.Debug("Configuring CORS ...");
            //        builder.Services.AddCors(options =>
            //        {
            //            options.AddPolicy("CorsPolicy", builder => builder
            //            .WithOrigins(appUI.FrontendOrigins.ToArray())
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials());
            //        });

            //        // Add controllers to container
            //        logger.Debug("Registering Controllers ...");
            //        builder.Services.AddControllers(conf =>
            //        {
            //            conf.AllowEmptyInputInBodyModelBinding = true;
            //        });

            //        logger.Debug("Configuring Swagger ...");
            //        InitSwagger(builder);

            //        logger.Debug("Configuring JWT authentication ...");
            //        SetJWTAutentication(builder, appSecurity);

            //        logger.Debug("Configuring Localization ...");
            //        SetLocalization(builder);

            //        logger.Debug("Configuring Authorization ...");
            //        builder.Services.AddAuthorization(config =>
            //        {
            //            config.AddPolicy(Policies.Administrators, PoliciesProfiles.AdminPolicy());
            //            config.AddPolicy(Policies.Managers, PoliciesProfiles.ManagerPolicy());
            //            config.AddPolicy(Policies.Users, PoliciesProfiles.UserPolicy());
            //        });

            //        logger.Debug("Configuring SignalR ...");
            //        builder.Services.AddSignalR();

            //        logger.Debug("Building Application ...");
            //        var app = builder.Build();

            //        // Preload O.R.M. session
            //        logger.Debug("Preload O.R.M. ...");
            //        app.Services.GetService<NHibernate.ISessionFactory>();

            //        // Configure the HTTP request pipeline.
            //        logger.Debug("Initializing Swagger ...");

            //        // Update the Request.Scheme using X-Forwarded-Proto header to handle redirects and security behind a reverse proxy... it allow getting real client ip
            //        app.UseForwardedHeaders(new ForwardedHeadersOptions
            //        {
            //            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //        });

            //        app.UseSwagger();
            //        app.UseSwaggerUI();

            //        app.UseExceptionHandler(exceptionHandlerApp =>
            //        {
            //            exceptionHandlerApp.Run(async context =>
            //            {
            //                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //                context.Response.ContentType = Application.Json;
            //                var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();

            //                if (exceptionHandler != null)
            //                {
            //                    Log.Error(exceptionHandler.Error, "Unhandled exception");
            //                    var response = new ResponseViewModel<string>
            //                    {
            //                        Success = false,
            //                        Value = "Unhandled exception",
            //                        ErrorMessage = exceptionHandler.Error.Message
            //                    };
            //                    await context.Response.WriteAsJsonAsync(response);
            //                }
            //            });
            //        });

            //        logger.Debug("Executing Services Initialization ...");

            //        var initializedServicesResult = app.InitServices();
            //        SerilogExtensions.LogResponsesResult(initializedServicesResult, "Services Initialization Result");

            //        //app.UseHttpsRedirection();
            //        logger.Debug("Initializing CORS ...");
            //        app.UseCors("CorsPolicy");

            //        logger.Debug("Initializing Authentication ...");
            //        app.UseAuthentication();
            //        app.UseAuthorization();

            //        // Add extra log properties
            //        app.UseMiddleware<SerilogMiddleware>();

            //        app.MapControllers();

            //        // Signal R
            //        app.MapHub<NotifyHub>($"{_signalRPathPrefix}/notifyHub");

            //        CreateWelcomeMessage(logger);
            //        app.Run();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Fatal(ex, "Application init fail!");
            //        Log.CloseAndFlush();
            //    }
            //}

        }
    }
}
