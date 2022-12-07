using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.ViewModels.Auth
{
    /// <summary>
    /// The view model is returned after the refresh request. It contains the new Access Token and a date with Refresh Token expiration date.
    /// </summary>
    public interface IRefreshTokenViewModel
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        string AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        string RefreshToken { get; set; }
        /// <summary>
        /// Gets or sets the refresh tokent expire.
        /// </summary>
        /// <value>
        /// The refresh tokent expire.
        /// </value>
        DateTime RefreshTokenExpire { get; set; }
    }
}
