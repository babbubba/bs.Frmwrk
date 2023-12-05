/// <summary>
/// 
/// </summary>
namespace bs.Frmwrk.Core.Models.Base
{
    /// <summary>
    /// Model that can be enabled or not
    /// </summary>
    public interface IEnableable
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IEnableable"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; set; }
    }
}