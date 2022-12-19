using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Test.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace bs.Frmwrk.Test
{
    public class ApiTest
    {
        [Test]
        public async Task BootstrapApi_Test()
        {
            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();
            Assert.That(authService, Is.Not.Null, "Cannot resolve AuthService from DI");

            var authResponse = await authService.AuthenticateAsync(new AuthRequestDto ("admin",  "admin" ), "test-host");
            Assert.That(authResponse, Is.Not.Null, "AuthenticateAsync doesnt work properly");
        }
    }
}