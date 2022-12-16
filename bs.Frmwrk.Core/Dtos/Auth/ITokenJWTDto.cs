namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenJWTDto
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        string Token { get; set; }
        /// <summary>
        /// Gets or sets the expire at.
        /// </summary>
        /// <value>
        /// The expire at.
        /// </value>
        DateTime ExpireAt { get; set; }
    }
}