using bs.Frmwrk.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace bs.Frmwrk.Core.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public interface IAuthController
    {
        /// <summary>
        /// Changes the password after forget password request or directly by logged in user.
        /// </summary>
        /// <param name="changeUserPasswordDto">The change user password dto.</param>
        /// <returns></returns>
        Task<IActionResult> ChangePassword(IChangeUserPasswordDto changeUserPasswordDto);

        /// <summary>
        /// Confirms the email after new user registration.
        /// </summary>
        /// <param name="confirmEmailDto">The confirm email dto.</param>
        /// <returns></returns>
        Task<IActionResult> ConfirmEmail(IConfirmEmailDto confirmEmailDto);

        /// <summary>
        /// Logins the specified authentication request dto.
        /// </summary>
        /// <param name="authRequestDto">The authentication request dto.</param>
        /// <returns></returns>
        Task<IActionResult> Login(IAuthRequestDto authRequestDto);

        /// <summary>
        /// Request the link to recover the password.
        /// </summary>
        /// <param name="requestRecoveryUserPasswordLinkDto">The request recovery user password link dto.</param>
        /// <returns></returns>
        Task<IActionResult> RecoveryPassword(IRequestRecoveryUserPasswordLinkDto requestRecoveryUserPasswordLinkDto);

        /// <summary>
        /// Registers a new user register dto.
        /// </summary>
        /// <param name="registerDto">The register dto.</param>
        /// <returns></returns>
        Task<IActionResult> Register(IAuthRegisterDto registerDto);
    }
}