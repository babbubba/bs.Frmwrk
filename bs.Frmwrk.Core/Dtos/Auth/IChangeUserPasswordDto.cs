namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    /// The DTO for change user password
    /// </summary>
    public interface IChangeUserPasswordDto
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
        /// Gets or sets the password confirm.
        /// </summary>
        /// <value>
        /// The password confirm.
        /// </value>
        string PasswordConfirm { get; set; }

        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>
        /// The old password.
        /// </value>
        string? OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the recovery password identifier.
        /// </summary>
        /// <value>
        /// The recovery password identifier.
        /// </value>
        string? RecoveryPasswordId { get; set; }
    }
}