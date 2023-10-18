using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    public class AuthRequestDto : IAuthRequestDto
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the recaptcha token.
        /// </summary>
        /// <value>
        /// The recaptcha token.
        /// </value>
        public string? RecaptchaToken { get; set; }
    }
}