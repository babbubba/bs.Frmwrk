using bs.Frmwrk.Application.Middlewares;
using bs.Frmwrk.Core.Globals.Config;
using bs.Frmwrk.Core.Models.Configuration;
using bs.Frmwrk.Core.Services.Base;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Reflection;

namespace bs.Frmwrk.Application
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureFrmwrk(this WebApplication app)
        {
            Log.Debug("Framework configuration...");

            app.PreloadORM();
            app.HandleReverseProxyRequest();
            app.ConfigureSwagger();
            app.ConfigureExceptionHandler();
            app.InitServices();
            app.UseCors(CorsConfigurations.CorsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<SerilogMiddleware>();
            app.MapControllers();
            app.MapSignalRHubs();

            Log.Debug("Framework configuration complete");
            Log.Information("Framework configured, application is starting");
        }

        internal static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandler != null)
                    {
                        Log.Error(exceptionHandler.Error, "Unhandled exception");
                        var response = new ApiResponse<string>
                        {
                            Success = false,
                            Value = "Unhandled exception",
                            ErrorMessage = exceptionHandler.Error.Message
                        };
                        await context.Response.WriteAsJsonAsync(response);
                    }
                });
            });
        }

        internal static void ConfigureSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        internal static void HandleReverseProxyRequest(this WebApplication app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        internal static void InitServices(this WebApplication app)
        {
            var result = new Dictionary<string, IApiResponse>();

            //var initializableServices = typeof(IInitializableService).GetImplTypesFromInterface();
            var initializableServices = typeof(IInitializableService).GetImplTypesFromInterface().Select(x => new Tuple<int, Type?>((int?)x?.GetProperty("InitPriority")?.GetValue(null, null) ?? 0, x));

            foreach (var initializableServiceTuple in initializableServices.OrderBy(s => s.Item1))
            {
                var initializableService = initializableServiceTuple.Item2;

                if (initializableService is null) continue;
                using (var scope = app.Services.CreateScope())
                {
                    var initializableServiceInterfaces = initializableService.GetInterfacesOf(new Type[] { typeof(IBsService), typeof(IInitializableService) });
                    if (initializableServiceInterfaces is not null && initializableServiceInterfaces.Count() > 1)
                    {
                        result.Add(initializableService.FullName ?? initializableService.Name, new ApiResponse<bool>(false, $"There are more than one interface for the class: '{initializableService.FullName ?? initializableService.Name}' ({string.Join(", ", initializableServiceInterfaces.Select(i => i.FullName ?? i.Name))})"));
                        continue;
                    }
                    else if (initializableServiceInterfaces is null)
                    {
                        result.Add(initializableService.FullName ?? initializableService.Name, new ApiResponse<bool>(false, $"There are no interface for the class: '{initializableService.FullName ?? initializableService.Name}'"));
                        continue;
                    }
                    var initializableServiceInterface = initializableServiceInterfaces.Single();

                    var serviceToInit = scope.ServiceProvider.GetService(initializableServiceInterface);

                    if (serviceToInit != null)
                    {
                        result.Add(initializableService.FullName ?? initializableService.Name, ((IInitializableService)serviceToInit).InitServiceAsync().Result);
                    }
                    else
                    {
                        result.Add(initializableService.FullName ?? initializableService.Name, new ApiResponse<bool>(false, "Cannot resolve instance from DI Container."));
                    }
                }
            }

            if (result.Any(r => !r.Value.Success))
            {
                foreach (var errorMessage in result.Where(r => !r.Value.Success).Select(r => $"Error initalizing service '{r.Key}': {r.Value.ErrorMessage}"))
                {
                    Log.Error(errorMessage);
                }
            }
        }

        internal static void MapSignalRHubs(this WebApplication app)
        {
            //TODO: Controlla se la registrazione dinamica degli HUB Signal R funziona veramente
            var coreSettings = app.Services.GetService<ICoreSettings>();

            if (coreSettings?.SignalRHubs is null)
            {
                return;
            }

            Log.Debug("Mapping SignalR Hubs");

            foreach (var signalRHub in coreSettings.SignalRHubs)
            {
                var hubClass = ReflectionExtensions.GetTypeByFullName(signalRHub.Key);

                if (hubClass is null)
                {
                    Log.Warning($"No implementation of the hub '{signalRHub.Key}' was found. This hub will never start");
                    continue;
                }

                MethodInfo? method = typeof(HubEndpointRouteBuilderExtensions).GetMethod(nameof(HubEndpointRouteBuilderExtensions.MapHub), BindingFlags.Static | BindingFlags.Public, new Type[] { typeof(IEndpointRouteBuilder), typeof(string) });
                if (method is null)
                {
                    Log.Error($"No implementation was found for the MapHub extension (check framework version). The hub '{signalRHub.Key}' will never start");
                    continue;
                }
                try
                {
                    MethodInfo generic = method.MakeGenericMethod(hubClass);
                    generic.Invoke(null, new object[] { app, UrlsConfigurations.SignalRHubPrefix + signalRHub.Value });
                }
                catch (Exception ex)
                {
                    Log.Error($"Cannot map the SignalR hub '{signalRHub.Key}'", ex);
                    continue;
                }
                Log.Debug($"SignalR Hub '{signalRHub.Key}' succefully mapped at url: '{UrlsConfigurations.SignalRHubPrefix + signalRHub.Value}'");
            }
        }

        internal static void PreloadORM(this WebApplication app)
        {
            app.Services.GetService<NHibernate.ISessionFactory>();
        }
    }
}