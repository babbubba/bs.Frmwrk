namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    /// The dto to request password recovery
    /// </summary>
    public interface IRequestRecoveryUserPasswordLinkDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; set; }
    }
}