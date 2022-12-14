using bs.Frmwrk.Application;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace bs.Frmwrk.Test
{
    public class Program
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

            var builder = WebApplication.CreateBuilder(args);
            try
            {
                builder.InitFrmwrk();
                builder.SetMailing();
                builder.SetFileSystem();
                builder.LoadExternalDll();
                builder.RegisterRepositories();
                builder.RegisterServices();

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
        }
    }
}