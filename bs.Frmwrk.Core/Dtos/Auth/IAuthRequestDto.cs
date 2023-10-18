namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface IAuthRequestDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the recaptcha token.
        /// </summary>
        /// <value>
        /// The recaptcha token.
        /// </value>
        string? RecaptchaToken { get; set; }
    }
}