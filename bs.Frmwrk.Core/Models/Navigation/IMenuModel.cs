using bs.Frmwrk.Core.Mapper.Profiles;

namespace bs.Frmwrk.Core.Models.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Mapper.Profiles.IIdentified" />
    public interface IMenuModel : IIdentified
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        string Code { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        ICollection<IMenuItemModel> Items { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }
    }
}