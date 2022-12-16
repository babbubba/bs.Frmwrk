using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Auth.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Auth.IRefreshTokenViewModel" />
    public class RefreshTokenViewModels : IRefreshTokenViewModel
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh tokent expire.
        /// </summary>
        /// <value>
        /// The refresh tokent expire.
        /// </value>
        public DateTime RefreshTokenExpire { get; set; }
    }
}