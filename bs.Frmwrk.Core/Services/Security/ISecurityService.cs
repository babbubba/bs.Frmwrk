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
        /// Gets the password score.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        IApiResponse<ISelectListItem> GetPasswordScore(string password);

        /// <summary>
        /// Checks the password validity.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        bool CheckPasswordValidity(string? password, out string? errorMessage);

        /// <summary>
        /// Checks the user permission asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        Task<bool> CheckUserPermissionAsync(IPermissionedUser user, string permissionCode, PermissionType type = PermissionType.None);

        /// <summary>
        /// Checks the user role asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roleCode">The role code.</param>
        /// <returns></returns>
        Task<bool> CheckUserRoleAsync(IRoledUser user, string roleCode);

        /// <summary>
        /// Tracks the login fail asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="clientIp">The client ip.</param>
        /// <returns></returns>
        Task TrackLoginFailAsync(string username, string? clientIp);

        /// <summary>
        /// Creates the permission if not exists asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        Task<IPermissionModel> CreatePermissionIfNotExistsAsync(ICreatePermissionDto dto);

        /// <summary>
        /// Sends the registration confirm link via mail if is enabled the VerifyEmail check in settings
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task SendRegistrationConfirmAsync(IUserModel model);

        /// <summary>
        /// Sends the recovery password link asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task SendRecoveryPasswordLinkAsync(IUserModel user);
        /// <summary>
        /// Adds the permission to user asynchronous.
        /// </summary>
        /// <param name="permissionCode">The permission code.</param>
        /// <param name="user">The user.</param>
        /// <param name="permissionType">Type of the permission.</param>
        /// <returns></returns>
        Task AddPermissionToUserAsync(string permissionCode, IPermissionedUser user, PermissionType? permissionType);
    }
}