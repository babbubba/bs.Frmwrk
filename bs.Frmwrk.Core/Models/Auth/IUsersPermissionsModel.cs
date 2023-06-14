﻿using bs.Frmwrk.Core.Mapper.Profiles;

namespace bs.Frmwrk.Core.Models.Auth
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.Mapper.Profiles.IIdentified" />
    public interface IUsersPermissionsModel : IIdentified
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        IUserModel User { get; set; }

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