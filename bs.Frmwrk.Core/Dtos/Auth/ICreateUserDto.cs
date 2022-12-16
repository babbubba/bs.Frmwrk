namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICreateUserDto
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
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string Password { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ICreateUserDto"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the roles ids.
        /// </summary>
        /// <value>
        /// The roles ids.
        /// </value>
        string[]? RolesIds { get; set; }
    }
}