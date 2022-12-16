using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Security;

namespace bs.Frmwrk.Core.Services.Security
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Checks the password validity.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        bool CheckPasswordValidity(string password, out string? errorMessage);

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

    }
}