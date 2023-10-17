using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Dtos.Security;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Services.Base;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Common;

namespace bs.Frmwrk.Core.Services.Security
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityService : IInitializableService
    {
        /// <summary>
        /// Occurs when [security event].
        /// </summary>
        event EventHandler<ISecurityEventDto>? SecurityEvent;

        /// <summary>
        /// Occurs when [too many attempts event].
        /// </summary>
        event EventHandler<ISecurityEventDto>? TooManyAttemptsEvent;

        /// <summary>
        /// Adds the permission to role asynchronous.
        /// </summary>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="role">The role.</param>
        /// <param name="permissionType">Type of the permission.</param>
        /// <returns></returns>
        Task AddPermissionToRoleAsync(string permissionCode, IRoleModel role, PermissionType? permissionType);

        /// <summary>
        /// Adds the permission to user asynchronous.
        /// </summary>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="user">The user.</param>
        /// <param name="permissionType">Type of the permission.</param>
        /// <returns></returns>
        Task AddPermissionToUserAsync(string permissionCode, IUserModel user, PermissionType? permissionType);

        /// <summary>
        /// Adds the permission to user asynchronous.
        /// </summary>
        /// <param name="permissionsCode">The permissions code.</param>
        /// <param name="user">The user.</param>
        /// <param name="permissionType">Type of the permission.</param>
        /// <returns></returns>
        Task AddPermissionsToUserAsync(string[]? permissionsCode, IUserModel user, PermissionType? permissionType);

        /// <summary>
        /// Checks the password validity.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        bool CheckPasswordValidity(string? password, out string? errorMessage);

        /// <summary>
        /// Checks the user permission asynchronous (if user has role admin it always return true).
        /// If the user role implement IPermissionedRole this will check if requested permission code is valid for the role's permissions.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Task<bool> CheckUserPermissionAsync(IUserModel? user, string permissionCode, PermissionType type = PermissionType.None);

        ///// <summary>
        ///// Checks the user permission asynchronous.
        ///// </summary>
        ///// <param name="user">The user.</param>
        ///// <param name="requiredPermissionsCodes">The required permissions codes.</param>
        ///// <param name="type">The type.</param>
        ///// <returns></returns>
        //Task<bool> CheckUserPermissionAsync(IPermissionedUser user, string[] requiredPermissionsCodes, PermissionType type = PermissionType.None);

        /// <summary>
        /// Checks the user role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleCode">The role code.</param>
        /// <returns></returns>
        Task<bool> CheckUserRoleAsync(IRoledUser? user, string roleCode);

        /// <summary>
        /// Checks if the user is memebership of one of the roles specified asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="rolesCode">The roles code.</param>
        /// <returns></returns>
        /// <exception cref="bs.Frmwrk.Core.Exceptions.BsException">2310051105 if user is not implementing roles</exception>
        Task<bool> CheckUserRolesAsync(IUserModel user, string[] rolesCode);

        /// <summary>
        /// Creates the permission if not exists asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        Task<IPermissionModel> CreatePermissionIfNotExistsAsync(ICreatePermissionDto dto);

        /// <summary>
        /// Gets the password score.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        IApiResponse<ISelectListItem> GetPasswordScore(string password);

        /// <summary>
        /// Sends the recovery password link asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task SendRecoveryPasswordLinkAsync(IUserModel user);

        /// <summary>
        /// Sends the registration confirm link via mail if is enabled the VerifyEmail check in settings
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task SendRegistrationConfirmAsync(IUserModel model);

        /// <summary>
        /// Tracks the login fail asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="clientIp">The client ip.</param>
        /// <returns></returns>
        Task TrackLoginFailAsync(string username, string? clientIp);

        Task AddRolesToUser(string[]? rolesCode, IUserModel? user);

        Task AddRolesToUser(Guid[]? rolesId, IUserModel? user);
        Task<bool> CheckGoogleRecaptcha(string token);
    }
}