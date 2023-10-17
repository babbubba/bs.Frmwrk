namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    /// The dto to request password recovery
    /// </summary>
    public interface IRequestRecoveryUserPasswordLinkDto
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; set; }
    }
}