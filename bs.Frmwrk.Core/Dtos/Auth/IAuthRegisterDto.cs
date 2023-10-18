namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface IAuthRegisterDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string? Password { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string? Email { get; set; }

        /// <summary>
        /// Gets or sets the recaptcha token from Google Recaptcha API (usually frontend ask for it then pass the token to backend in this property).
        /// </summary>
        /// <value>
        /// The recaptcha token.
        /// </value>
        string? RecaptchaToken { get; set; }
    }
}