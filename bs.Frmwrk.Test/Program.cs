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
            var builder = WebApplication.CreateBuilder(args);
            try
            {
                builder.InitFrmwrk();

                var app = builder.Build();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Log.Logger.Fatal("Application startup failed.", ex);
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