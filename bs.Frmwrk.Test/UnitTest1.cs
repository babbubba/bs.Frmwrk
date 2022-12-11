using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Services.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace bs.Frmwrk.Test
{
    public class AuthRequestDto : IAuthRequestDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class Tests
    {
        [Test]
        public async Task Test1()
        {
            var logger = Root.ServiceProvider?.GetRequiredService<ILogger<Tests>>();
            logger.LogInformation("Log something");

            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();

            var authResponse = await authService?.AuthenticateAsync(new AuthRequestDto { UserName = "admin", Password = "admin" }, "test-host");

            Assert.IsTrue(authResponse.Success, authResponse.ErrorMessage);
        }
    }
}