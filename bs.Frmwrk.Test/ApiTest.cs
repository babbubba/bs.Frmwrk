using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Shared;
using bs.Frmwrk.Test.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace bs.Frmwrk.Test
{
    public class ApiTest
    {
        /// <summary>
        /// Resolves the authentication service and try to login.
        /// </summary>
        [Test]
        public async Task ResolveAuthService()
        {
            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();
            Assert.That(authService, Is.Not.Null, "Cannot resolve AuthService from DI");

            var authResponse = await authService.AuthenticateAsync(new AuthRequestDto ("admin",  "admin" ), "test-host");
            Assert.That(authResponse, Is.Not.Null, "AuthenticateAsync doesnt work properly");
        }

        [Test]
        public async Task ResolveSecurityService()
        {
            var securityService = Root.ServiceProvider?.GetRequiredService<ISecurityService>();
            Assert.That(securityService, Is.Not.Null, "Cannot resolve SecurityService from DI");

            var getPasswordScoreResponse = securityService.GetPasswordScore("pippo");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.VeryWeak), "GetPasswordScore doesnt work properly");

            getPasswordScoreResponse = securityService.GetPasswordScore("giameglio");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.Weak), "GetPasswordScore doesnt work properly");

            getPasswordScoreResponse = securityService.GetPasswordScore("giamegl!o");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.Medium), "GetPasswordScore doesnt work properly");

            getPasswordScoreResponse = securityService.GetPasswordScore("decisamentegiameglio");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.Medium), "GetPasswordScore doesnt work properly");

            getPasswordScoreResponse = securityService.GetPasswordScore("decisamentegiam3glio");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.Strong), "GetPasswordScore doesnt work properly");

            getPasswordScoreResponse = securityService.GetPasswordScore("decisamentegiameglio!");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.Strong), "GetPasswordScore doesnt work properly");

            getPasswordScoreResponse = securityService.GetPasswordScore("decIsamentegiameglio!");
            Assert.That(getPasswordScoreResponse, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value, Is.Not.Null, "GetPasswordScore doesnt work properly");
            Assert.That(getPasswordScoreResponse.Value.Id.ToEnum<PasswordScore>(), Is.EqualTo(PasswordScore.VeryStrong), "GetPasswordScore doesnt work properly");
        }
    }
}