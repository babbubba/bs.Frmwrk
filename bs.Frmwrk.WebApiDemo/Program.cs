using bs.Frmwrk.Application;
using bs.Frmwrk.Core.Exceptions;
using Serilog;

namespace bs.Frmwrk.WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            try
            {
                builder.BootstrapFrmwrk();

                var app = builder.Build();

                app.ConfigureFrmwrk();

                app.Run();
            }
            catch (BsException ex)
            {
                Log.Logger.Fatal($"Application startup failed ({ex.ErrorCode ?? 0}): {ex.GetBaseException().Message}", ex);
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal($"Application startup failed: {ex.GetBaseException().Message}", ex);
            }
            finally
            {
                Log.Information("Shut down complete");
                Log.CloseAndFlush();
            }



        }
    }
}