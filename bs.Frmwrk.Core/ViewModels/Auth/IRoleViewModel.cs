namespace bs.Frmwrk.Core.ViewModels.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface IRoleViewModel
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        string Code { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }

        /// <summary>
        /// Gets or sets the roles permissions.
        /// </summary>
        /// <value>
        /// The roles permissions.
        /// </value>
        ICollection<IPermissionViewModel>? RolesPermissions { get; set; }
    }
}