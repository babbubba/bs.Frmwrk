namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfirmEmailDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        string UserId { get; set; }
        /// <summary>
        /// Gets or sets the confirmation identifier.
        /// </summary>
        /// <value>
        /// The confirmation identifier.
        /// </value>
        string ConfirmationId { get; set; }
    }
}