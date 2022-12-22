using bs.Frmwrk.Core.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core
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
        /// Registers the specified register dto.
        /// </summary>
        /// <param name="registerDto">The register dto.</param>
        /// <returns></returns>
        Task<IActionResult> Register(IAuthRegisterDto registerDto);

        /// <summary>
        /// Logins the specified authentication request dto.
        /// </summary>
        /// <param name="authRequestDto">The authentication request dto.</param>
        /// <returns></returns>
        Task<IActionResult> Login(IAuthRequestDto authRequestDto);
    }
}
