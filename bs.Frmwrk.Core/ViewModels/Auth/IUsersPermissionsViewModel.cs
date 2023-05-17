using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Core.ViewModels.Auth
{
    public interface IUsersPermissionsViewModel
    {
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
        IPermissionViewModel Permission { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        PermissionType Type { get; set; }
    }

}