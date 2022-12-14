using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Test.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace bs.Frmwrk.Test
{

    public class ApiTest
    {
        [Test]
        public async Task BootstrapApi_Test()
        {

            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();

            var authResponse = await authService?.AuthenticateAsync(new AuthRequestDto { UserName = "admin", Password = "admin" }, "test-host");

            Assert.IsNotNull(authResponse);
           // Assert.IsTrue(authResponse.Success, authResponse.ErrorMessage);
        }
    }
}