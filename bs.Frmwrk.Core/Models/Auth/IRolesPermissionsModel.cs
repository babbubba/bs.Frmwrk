using bs.Frmwrk.Core.Mapper.Profiles;

namespace bs.Frmwrk.Core.Models.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface IRolesPermissionsModel : IIdentified
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        IPermissionedRole Role { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
        IPermissionModel Permission { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        PermissionType Type { get; set; }
    }
}