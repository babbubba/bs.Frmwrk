using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Core.Services.Auth
{

    public interface IAuthService
    {
        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="userAuth">The user authentication.</param>
        /// <param name="clientIp">The client ip.</param>
        /// <returns></returns>
        Task<IApiResponseViewModel<IUserViewModel>> AuthenticateAsync(IAuthRequestDto authRequest, string? clientIp);

        /// <summary>
        /// Keeps alive the user setting last ping to current date time (UTC).
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Task<IApiResponseViewModel> KeepAliveAsync(IKeepedAliveUser user);

        /// <summary>
        /// Refreshes the access token asynchronous.
        /// </summary>
        /// <param name="refreshTokenDto">The refresh token dto.</param>
        /// <returns></returns>
        Task<IApiResponseViewModel<IRefreshTokenViewModel>> RefreshAccessTokenAsync(IRefreshTokenRequestDto refreshTokenRequest);

        /// <summary>
        /// Creates the user asynchronous and return the new created user's id.
        /// </summary>
        /// <param name="createUserDto">The create user dto.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns></returns>
        Task<IApiResponseViewModel<string>> CreateUserAsync(ICreateUserDto createUserDto, IUserModel currentUser);

    }
}