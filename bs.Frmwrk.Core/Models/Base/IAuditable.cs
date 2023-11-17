using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Core.Models.Base
{
    /// <summary>
    /// Model that owns last modify (date and user)
    /// </summary>
    public interface IAuditable
    {

        /// <summary>
        /// Gets or sets the last change date.
        /// </summary>
        /// <value>
        /// The last change date.
        /// </value>
        DateTime LastChangeDate { get; set; }
        /// <summary>
        /// Gets or sets the last change by.
        /// </summary>
        /// <value>
        /// The last change by.
        /// </value>
        IUserModel LastChangeBy { get; set; }
    }
}