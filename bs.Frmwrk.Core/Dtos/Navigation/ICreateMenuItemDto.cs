namespace bs.Frmwrk.Core.Dtos.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICreateMenuItemDto
    {
        /// <summary>
        /// Gets or sets the authorized roles code.
        /// </summary>
        /// <value>
        /// The authorized roles code.
        /// </value>
        string[] AuthorizedRolesCode { get; set; }
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
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string? Id { get; set; }
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
        /// Gets or sets the parent item code.
        /// </summary>
        /// <value>
        /// The parent item code.
        /// </value>
        string? ParentItemCode { get; set; }
        /// <summary>
        /// Gets or sets the parent menu code.
        /// </summary>
        /// <value>
        /// The parent menu code.
        /// </value>
        string ParentMenuCode { get; set; }
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
        /// Gets or sets the required permissions code.
        /// </summary>
        /// <value>
        /// The required permissions code.
        /// </value>
        string[] RequiredPermissionsCode { get; set; }
        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        /// <value>
        /// The tool tip.
        /// </value>
        string? ToolTip { get; set; }
    }
}