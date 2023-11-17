using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Base;

namespace bs.Frmwrk.Core.Models.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Base.IIdentified" />
    public interface IMenuItemModel : IIdentified
    {
        /// <summary>
        /// Gets or sets the authorized roles.
        /// </summary>
        /// <value>
        /// The authorized roles.
        /// </value>
        IList<IRoleModel> AuthorizedRoles { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        string Code { get; set; }
        /// <summary>
        /// Gets or sets the CSS class.
        /// </summary>
        /// <value>
        /// The CSS class.
        /// </value>
        string? CssClass { get; set; }
        /// <summary>
        /// Gets or sets the icon style.
        /// </summary>
        /// <value>
        /// The icon style.
        /// </value>
        string? IconStyle { get; set; }
        /// <summary>
        /// Gets or sets the is default open.
        /// </summary>
        /// <value>
        /// The is default open.
        /// </value>
        bool? IsDefaultOpen { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets the is TreeView.
        /// </summary>
        /// <value>
        /// The is TreeView.
        /// </value>
        bool? IsTreeView { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }
        /// <summary>
        /// Gets or sets the parent item.
        /// </summary>
        /// <value>
        /// The parent item.
        /// </value>
        IMenuItemModel? ParentItem { get; set; }
        /// <summary>
        /// Gets or sets the parent menu.
        /// </summary>
        /// <value>
        /// The parent menu.
        /// </value>
        IMenuModel ParentMenu { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        string Path { get; set; }
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        int Position { get; set; }
        /// <summary>
        /// Gets or sets the required permissions.
        /// </summary>
        /// <value>
        /// The required permissions.
        /// </value>
        IList<IPermissionModel> RequiredPermissions { get; set; }
        /// <summary>
        /// Gets or sets the sub items.
        /// </summary>
        /// <value>
        /// The sub items.
        /// </value>
        ICollection<IMenuItemModel>? SubItems { get; set; }
        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        string? ToolTip { get; set; }
    }
}