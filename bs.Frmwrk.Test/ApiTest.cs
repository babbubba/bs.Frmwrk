using bs.Data.Interfaces;
using bs.Frmwrk.Auth.Dtos;
using bs.Frmwrk.Core.Dtos.Navigation;
using bs.Frmwrk.Core.Globals.Auth;
using bs.Frmwrk.Core.Globals.Security;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Navigation;
using bs.Frmwrk.Core.Services.Security;
using bs.Frmwrk.Navigation.Dtos;
using bs.Frmwrk.Security.Dtos;
using bs.Frmwrk.Security.Services;
using bs.Frmwrk.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;

namespace bs.Frmwrk.Test
{
    public class ApiTest
    {
        private static string EMAIL_TEST = "fcavallari@bsoftsolutions.it";

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
            var r5 = await authService.AuthenticateAsync(new AuthRequestDto { UserName = "admin", Password = "Pa$$w0rd01!" }, "test-host");
            Assert.That(r5, Is.Not.Null, "AuthenticateAsync doesnt work properly");
            Assert.That(r5.Success, Is.True, "Admin authentication fails");

            log?.LogInformation("Testing authentication (user 'User')");
            var r6 = await authService.AuthenticateAsync(new AuthRequestDto() { UserName = "user", Password = "Pa$$w0rd01!" }, "test-host");
            Assert.That(r6, Is.Not.Null, "AuthenticateAsync doesnt work properly");
            Assert.That(r6.Success, Is.True, "User authentication fails");

            //Create user
            log?.LogInformation("Registering new user (user 'test')");
            var newUser = new AuthRegisterDto { UserName = "test", Password = "Passw0rdDiProva@", Email = EMAIL_TEST };
            var r7 = await authService.RegisterNewUserAsync(newUser, new string[] { PermissionsCodes.USERS_REGISTRY, PermissionsCodes.ROLES_REGISTRY }, new string[] { RolesCodes.ADMINISTRATOR, RolesCodes.USERS });

            Assert.That(r7, Is.Not.Null, "CreateUserAsync doesnt work properly");
            Assert.That(r7.Success, Is.True, $"Cannot create the user: {r7.ErrorMessage} ({r7.ErrorCode})");

            // Try to receive the forgotten password
            //log?.LogInformation("Registering new user (user 'test')");
            //var r8 = await authService.RequestRecoveryUserPasswordLinkAsync(new RequestRecoveryUserPasswordLinkDto { UserName = "user", Email = "user@test.com" });
            //Assert.That(r8, Is.Not.Null, "RequestRecoveryUserPasswordLinkAsync doesnt work properly");
            //Assert.That(r8.Success, Is.True, $"Cannot recover the user's password: {r8.ErrorMessage} ({r8.ErrorCode})");
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

        //[Test]
        //public async Task ChangePassword_Test()
        //{
        //    var log = Root.ServiceProvider?.GetRequiredService<ILogger<ApiTest>>();
        //    var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();
        //    Assert.That(authService, Is.Not.Null, "Cannot resolve AuthService from DI");

        //    var uow = Root.ServiceProvider?.GetRequiredService<IUnitOfWork>();
        //    Assert.That(uow, Is.Not.Null, "Cannot resolve Unit of Work from DI");

        //    var testUser = await uow.Session.Query<IUserModel>().SingleOrDefaultAsync(u => u.UserName == "user");
        //    Assert.That(testUser, Is.Not.Null, "Cannot find 'user' user in the database");

        //    var r1 = await authService.ChangePasswordAsync(new ChangeUserPasswordDto { OldPassword = "Pa$$w0rd01!", Password = "Pa$$w0rd02!", PasswordConfirm = "Pa$$w0rd02!", UserName = "user" }, testUser);
        //    Assert.That(r1, Is.Not.Null, "ChangePasswordAsync doesnt work properly");
        //    Assert.That(r1.Success, Is.True, $"Cannot create the user: {r1.ErrorMessage} ({r1.ErrorCode})");
        //}

        [Test]
        public async Task Permissioned_Role_Test()
        {
            var log = Root.ServiceProvider?.GetRequiredService<ILogger<ApiTest>>();

            log?.LogInformation("Testing IAuthService resolution");
            var authService = Root.ServiceProvider?.GetRequiredService<IAuthService>();
            Assert.That(authService, Is.Not.Null, "Cannot resolve AuthService from DI");

            log?.LogInformation("Testing ISecurityService resolution");
            var securityService = Root.ServiceProvider?.GetRequiredService<ISecurityService>();
            Assert.That(securityService, Is.Not.Null, "Cannot resolve SecurityService from DI");

            var uow = Root.ServiceProvider?.GetRequiredService<IUnitOfWork>();
            Assert.That(uow, Is.Not.Null, "Cannot resolve UnitOdWork from DI");

            // Create the TEST_ROLE wirh the USERS_REGISTRY permission
            var testRole = await authService.CreateRoleIfNotExistsAsync(new CreateRoleDto("TEST_ROLE", "Role created for testing purpose", new string[] { "USERS_REGISTRY" }));
            Assert.That(testRole, Is.Not.Null, "Cannot create the TEST_ROLE role");
        }

        [Test]
        public async Task NavigationService_Test()
        {
            var log = Root.ServiceProvider?.GetRequiredService<ILogger<ApiTest>>();

            log?.LogInformation("Testing INavigationService resolution");
            var navigationService = Root.ServiceProvider?.GetRequiredService<INavigationService>();
            Assert.That(navigationService, Is.Not.Null, "Cannot resolve INavigationService from DI");

            log?.LogInformation("Testing menu creation");
            var menuD = new CreateMenuDto
            {
                Code = "MainSidebar",
                Label = "Sidebar di sinistra",
                IsEnabled = true
            };

            var menuVm = await navigationService.CreateUpdateMenuAsync(menuD, await GetCurrentUser());
            Assert.That(menuVm, Is.Not.Null, "CreateUpdateMenuAsync doesnt work properly");
            Assert.That(menuVm.Success, Is.True, "Creating menu fails");

            log?.LogInformation("Testing menu update");

            menuD.Id = menuVm.Value.Id;
            menuD.Label = "Sidebar di sinistra (modificato)";

            var menuVmUpdated = await navigationService.CreateUpdateMenuAsync(menuD, await GetCurrentUser());
            Assert.That(menuVm, Is.Not.Null, "CreateUpdateMenuAsync doesnt work properly");
            Assert.That(menuVm.Success, Is.True, "Updating menu fails");

            log?.LogInformation("Testing menu item creation");
            var menuItemD = new CreateMenuItemDto
            {
                ParentMenuCode = "MainSidebar",
                Code = "item-1",
                AuthorizedRolesCode = new[] { Core.Globals.Auth.RolesCodes.ADMINISTRATOR, Core.Globals.Auth.RolesCodes.USERS },
                RequiredPermissionsCode = new[] { PermissionsCodes.ROLES_REGISTRY, PermissionsCodes.USERS_MODERATION },
                IsEnabled = true,
                Label = "Elemento 1 (padre)",
                IsTreeView = true,
                Path = "/route/item-1",
                Position = 1,
                ToolTip = "Questa è la voce di menu 1 che conterra altre voci di menu annidate"
            };

            var menuItemVm = await navigationService.CreateUpdateMenuItemAsync(menuItemD, await GetCurrentUser());
            Assert.That(menuItemVm, Is.Not.Null, "CreateUpdateMenuItemAsync doesnt work properly");
            Assert.That(menuItemVm.Success, Is.True, "Creating menu item fails");

            log?.LogInformation("Testing menu item update");

            menuItemD.Id = menuItemVm.Value.Id;
            menuItemD.RequiredPermissionsCode = new[] { PermissionsCodes.USERS_REGISTRY, PermissionsCodes.ROLES_REGISTRY };
            menuItemD.Label = "Elemento 1 (padre) [modificato]";

            var menuItemVmUpdated = await navigationService.CreateUpdateMenuItemAsync(menuItemD, await GetCurrentUser());
            Assert.That(menuItemVm, Is.Not.Null, "CreateUpdateMenuItemAsync doesnt work properly");
            Assert.That(menuItemVm.Success, Is.True, "Updating menu item fails");

            log?.LogInformation("Testing nested menu item create");
            var nestedMenuItemD = new CreateMenuItemDto
            {
                ParentMenuCode = "MainSidebar",
                Code = "sub-item-1",
                AuthorizedRolesCode = new[] { Core.Globals.Auth.RolesCodes.USERS },
                RequiredPermissionsCode = new[] { PermissionsCodes.ROLES_REGISTRY },
                IsEnabled = true,
                Label = "Sub Elemento 1 (figlio)",
                IsTreeView = true,
                Path = "/route/item-1/sub.item-1",
                Position = 1,
                ToolTip = "Questa è la voce di menu figlia della voce di menu 1",
                ParentItemCode = menuItemVm.Value.Code,
            };

            var nestedMenuItemVM = await navigationService.CreateUpdateMenuItemAsync(nestedMenuItemD, await GetCurrentUser());
            Assert.That(menuItemVm, Is.Not.Null, "CreateUpdateMenuItemAsync doesnt work properly");
            Assert.That(menuItemVm.Success, Is.True, "Creating nested menu item fails");

            log?.LogInformation("Testing retrieving menu (with permission and role check)");
            var miItem2 = new CreateMenuItemDto
            {
                ParentMenuCode = "MainSidebar",
                Code = "item-2",
                AuthorizedRolesCode = new[] { Core.Globals.Auth.RolesCodes.ADMINISTRATOR, Core.Globals.Auth.RolesCodes.USERS },
                IsEnabled = true,
                Label = "Elemento 2",
                IsTreeView = true,
                Path = "/route/item-2",
                Position = 1,
                ToolTip = "Questa è la voce di menu 2 che conterra altre voci di menu annidate"
            };

            var r22 = await navigationService.CreateUpdateMenuItemAsync(miItem2, await GetCurrentUser());

            var miItem2Sub1 = new CreateMenuItemDto
            {
                ParentMenuCode = "MainSidebar",
                Code = "item-2-sub-1",
                AuthorizedRolesCode = new[] { Core.Globals.Auth.RolesCodes.ADMINISTRATOR, Core.Globals.Auth.RolesCodes.USERS },
                RequiredPermissionsCode = new[] { PermissionsCodes.ROLES_REGISTRY },
                IsEnabled = true,
                Label = "Figlio 1",
                IsTreeView = false,
                Path = "/route/item-2-sub-1",
                Position = 1,
                ParentItemCode = r22.Value.Code,
            };

            await navigationService.CreateUpdateMenuItemAsync(miItem2Sub1, await GetCurrentUser());

            var getMenuItemsByMenuCodeResponse = await navigationService.GetMenuItemsByMenuCodeAsync("MainSidebar", await GetCurrentUser());

            Assert.That(getMenuItemsByMenuCodeResponse, Is.Not.Null, "GetMenuItemsByMenuCodeAsync doesnt work properly");
            Assert.That(getMenuItemsByMenuCodeResponse.Success, Is.True, "Retrieving menu items fails");
        }
    }
}