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
        static string EMAIL_TEST = "fcavallari@bsoftsolutions.it";

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
            var r5 = await authService.AuthenticateAsync(new AuthRequestDto("admin", "Pa$$w0rd01!"), "test-host");
            Assert.That(r5, Is.Not.Null, "AuthenticateAsync doesnt work properly");
            Assert.That(r5.Success, Is.True, "Admin authentication fails");

            log?.LogInformation("Testing authentication (user 'User')");
            var r6 = await authService.AuthenticateAsync(new AuthRequestDto("user", "Pa$$w0rd01!"), "test-host");
            Assert.That(r6, Is.Not.Null, "AuthenticateAsync doesnt work properly");
            Assert.That(r6.Success, Is.True, "User authentication fails");

            //Create user
            log?.LogInformation("Registering new user (user 'test')");
            var newUser = new AuthRegisterDto { UserName = "test", Password = "Passw0rdDiProva@", Email = EMAIL_TEST };
            var r7 = await authService.RegisterNewUserAsync(newUser);

            Assert.That(r7, Is.Not.Null, "CreateUserAsync doesnt work properly");
            Assert.That(r7.Success, Is.True, $"Cannot create the user: {r7.ErrorMessage} ({r7.ErrorCode})");

            // Try to receive the forgotten password
            log?.LogInformation("Registering new user (user 'test')");
            var r8 = await authService.RequestRecoveryUserPasswordLinkAsync(new RequestRecoveryUserPasswordLinkDto { UserName = "user", Email = "user@test.com" });
            Assert.That(r8, Is.Not.Null, "RequestRecoveryUserPasswordLinkAsync doesnt work properly");
            Assert.That(r8.Success, Is.True, $"Cannot recover the user's password: {r8.ErrorMessage} ({r8.ErrorCode})");
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


        [Test]
        public async Task ChangePassword_Test()
        {
            var log = Root.ServiceProvider?.GetRequiredService<ILogger<ApiTest>>();
            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();
            Assert.That(authService, Is.Not.Null, "Cannot resolve AuthService from DI");

            var uow = Root.ServiceProvider?.GetRequiredService<IUnitOfWork>();
            Assert.That(uow, Is.Not.Null, "Cannot resolve Unit of Work from DI");

            var testUser = await uow.Session.Query<IUserModel>().SingleOrDefaultAsync(u => u.UserName == "user");
            Assert.That(testUser, Is.Not.Null, "Cannot find 'user' user in the database");

            var r1 = await authService.ChangePasswordAsync(new ChangeUserPasswordDto { OldPassword = "Pa$$w0rd01!" , Password = "Pa$$w0rd02!",PasswordConfirm = "Pa$$w0rd02!", UserName = "user" }, testUser);
            Assert.That(r1, Is.Not.Null, "ChangePasswordAsync doesnt work properly");
            Assert.That(r1.Success, Is.True, $"Cannot create the user: {r1.ErrorMessage} ({r1.ErrorCode})");
        }

    }
}