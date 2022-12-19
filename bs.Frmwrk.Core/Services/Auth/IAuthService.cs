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
        /// Occurs when [login event].
        /// </summary>
        event EventHandler<IAuthEventDto>? LoginEvent;

        /// <summary>
        /// Occurs when [authentication event].
        /// </summary>
        event EventHandler<IAuthEventDto>? AuthEvent;

        /// <summary>
        /// Authenticates the user with the provided password and returns the user data and tokens.
        /// </summary>
        /// <param name="authRequest">The authentication request.</param>
        /// <param name="clientIp">The client ip.</param>
        /// <returns></returns>
        Task<IApiResponse<IUserViewModel>> AuthenticateAsync(IAuthRequestDto authRequest, string? clientIp);

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
        /// <param name="createUserDto">The create user dto.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponse<string>> CreateUserAsync(ICreateUserDto createUserDto, IUserModel currentUser);
    }
}