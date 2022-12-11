using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace bs.Frmwrk.Test
{
    public static class Root
    {
        private static WebApplicationFactory<Program>? application;
        private static HttpClient? client;

        public static HttpClient? Client
        {
            get
            {
                if (client == null)
                {
                    client = Application?.CreateClient();
                }
                return client;
            }
        }

        public static IServiceProvider? ServiceProvider
        {
            get => Application?.Services.CreateScope().ServiceProvider;
        }

        private static WebApplicationFactory<Program>? Application
        {
            get
            {
                if (application == null)
                {
                    application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { });
                }
                return application;
            }
        }
    }
}