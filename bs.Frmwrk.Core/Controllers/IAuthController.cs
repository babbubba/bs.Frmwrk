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
        /// Confirms the email.
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
        /// Registers the specified register dto.
        /// </summary>
        /// <param name="registerDto">The register dto.</param>
        /// <returns></returns>
        Task<IActionResult> Register(IAuthRegisterDto registerDto);
    }
}