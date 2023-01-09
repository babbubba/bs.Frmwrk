using bs.Data.Interfaces;
using bs.Frmwrk.Auth.Dtos;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Shared;
using bs.Frmwrk.Test.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;

namespace bs.Frmwrk.Test
{
    public class ApiTest
    {

        public async Task<IUserModel> GetCurrentUser()
        {
            var uow = Root.ServiceProvider?.GetRequiredService<IUnitOfWork>();
            return await uow.Session.Query<IUserModel>().SingleOrDefaultAsync(u => u.UserName == "admin");
        }

        [Test]
        public async Task AuthService_Test()
        {
            var log = Root.ServiceProvider?.GetRequiredService<ILogger<ApiTest>>();

            log?.LogInformation("Testing IAuthService resolution");
            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();
            Assert.That(authService, Is.Not.Null, "Cannot resolve AuthService from DI");

            log?.LogInformation("Testing authentication (user 'Admin')");
            var authResponse = await authService.AuthenticateAsync(new AuthRequestDto("admin", "Pa$$w0rd01!"), "test-host");
            Assert.That(authResponse, Is.Not.Null, "AuthenticateAsync doesnt work properly");
            Assert.That(authResponse.Success, Is.True, "Admin authentication fails");

            log?.LogInformation("Testing authentication (user 'User')");
            var authResponse2 = await authService.AuthenticateAsync(new AuthRequestDto("user", "Pa$$w0rd01!"), "test-host");
            Assert.That(authResponse2, Is.Not.Null, "AuthenticateAsync doesnt work properly");
            Assert.That(authResponse2.Success, Is.True, "User authentication fails");

            //Create user
            var newUser = new AuthRegisterDto { UserName = "test", Password = "Passw0rdDiProva@", Email = "fcavallari@bsoftsolutions.it" };
            var createUserResponse = await authService.RegisterNewUserAsync(newUser);

            Assert.That(createUserResponse, Is.Not.Null, "CreateUserAsync doesnt work properly");
            Assert.That(createUserResponse.Success, Is.True, $"Cannot create the user: {createUserResponse.ErrorMessage} ({createUserResponse.ErrorCode})");
        }

        [Test]
        public async Task SecurityService_Test()
        {
            var log = Root.ServiceProvider?.GetRequiredService<ILogger<ApiTest>>();

            log?.LogInformation("Testing ISecurityService resolution");
            var securityService = Root.ServiceProvider?.GetRequiredService<ISecurityService>();
            Assert.That(securityService, Is.Not.Null, "Cannot resolve SecurityService from DI");

            log?.LogInformation("Testing password management");
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

            log?.LogInformation("Testing permissions");
        }


    }
}