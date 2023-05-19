using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Core.Services.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Occurs when [authentication event].
        /// </summary>
        event EventHandler<IAuthEventDto>? AuthEvent;

        /// <summary>
        /// Occurs when [login event].
        /// </summary>
        event EventHandler<IAuthEventDto>? LoginEvent;

        /// <summary>
        /// Authenticates the user with the provided password and returns the user data and tokens.
        /// </summary>
        /// <param name="authRequest">The authentication request.</param>
        /// <param name="clientIp">The client ip.</param>
        /// <returns></returns>
        Task<IApiResponse<IUserViewModel>> AuthenticateAsync(IAuthRequestDto authRequest, string? clientIp);

        /// <summary>
        /// Changes the password for user asynchronous.
        /// </summary>
        /// <param name="changeUserPasswordDto">The change user password dto.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse> ChangePasswordAsync(IChangeUserPasswordDto changeUserPasswordDto, IUserModel? currentUser);

        /// <summary>
        /// Confirms the email asynchronous after new user registration.
        /// </summary>
        /// <param name="confirmEmailDto">The confirm email dto.</param>
        /// <returns></returns>
        Task<IApiResponse> ConfirmEmailAsync(IConfirmEmailDto confirmEmailDto);

        /// <summary>
        /// Creates the role if not exists asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        Task<IRoleModel> CreateRoleIfNotExistsAsync(ICreateRoleDto dto);

        /// <summary>
        /// Creates the user if not exists asynchronous.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <param name="isSystemUser">if set to <c>true</c> [is system user].</param>
        /// <returns></returns>
        Task<IUserModel> CreateUserIfNotExistsAsync(ICreateUserDto dto, bool isSystemUser = false);

        /// <summary>
        /// Keeps alive the user setting last ping to current date time (UTC).
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task<IApiResponse> KeepAliveAsync(IKeepedAliveUser user);

        /// <summary>
        /// Refreshes the access token asynchronous.
        /// </summary>
        /// <param name="refreshTokenRequest">The refresh token request.</param>
        /// <returns></returns>
        /// <param name="clientIp"></param>
        Task<IApiResponse<IRefreshTokenViewModel>> RefreshAccessTokenAsync(IRefreshTokenRequestDto refreshTokenRequest, string? clientIp = null);

        /// <summary>
        /// Creates the user asynchronous and return the new created user's id.
        /// </summary>
        /// <param name="authRegisterDto">The authentication register dto.</param>
        /// <param name="permissionsCodes">The permissions codes.</param>
        /// <returns></returns>
        //Task<IApiResponse<string>> RegisterNewUserAsync(IAuthRegisterDto authRegisterDto);
        Task<IApiResponse<string>> RegisterNewUserAsync(IAuthRegisterDto authRegisterDto, string[]? permissionsCodes);

        /// <summary>
        /// Recoveries the user password asynchronous.
        /// </summary>
        /// <param name="recoveryUserPasswordDto">The recovery user password dto.</param>
        /// <returns></returns>
        Task<IApiResponse> RequestRecoveryUserPasswordLinkAsync(IRequestRecoveryUserPasswordLinkDto recoveryUserPasswordDto);
    }
}