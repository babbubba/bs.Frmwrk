namespace bs.Frmwrk.Core.Dtos.Security
{
    /// <summary>
    ///
    /// </summary>
    public interface IRecaptchaResponseDto
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        string Action { get; set; }

        /// <summary>
        /// Gets or sets the check date.
        /// </summary>
        /// <value>
        /// The check date.
        /// </value>
        DateTime CheckDate { get; set; }

        /// <summary>
        /// Gets or sets the error codes.
        /// </summary>
        /// <value>
        /// The error codes.
        /// </value>
        string[] ErrorCodes { get; set; }

        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        /// <value>
        /// The hostname.
        /// </value>
        string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        decimal Score { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRecaptchaResponseDto"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        bool Success { get; set; }
    }
}