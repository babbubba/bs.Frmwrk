using bs.Frmwrk.Core.Dtos.Auth;

namespace bs.Frmwrk.Auth.Dtos
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Dtos.Auth.IAuthRegisterDto" />
    public class AuthRegisterDto : IAuthRegisterDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the recaptcha token from Google Recaptcha API (usually frontend ask for it then pass the token to backend in this property).
        /// </summary>
        /// <value>
        /// The recaptcha token.
        /// </value>
        public string? RecaptchaToken { get; set; }
    }
}