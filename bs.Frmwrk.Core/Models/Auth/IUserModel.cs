namespace bs.Frmwrk.Core.Models.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; set; }
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
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        string PasswordHash { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IUserModel"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the last login.
        /// </summary>
        /// <value>
        /// The last login.
        /// </value>
        DateTime? LastLogin { get; set; }
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        string? RefreshToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh token expire.
        /// </summary>
        /// <value>
        /// The refresh token expire.
        /// </value>
        DateTime? RefreshTokenExpire { get; set; }
        /// <summary>
        /// Gets or sets the last ip.
        /// </summary>
        /// <value>
        /// The last ip.
        /// </value>
        string? LastIp { get; set; }
    }
}