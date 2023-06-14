namespace bs.Frmwrk.Core.Models.Auth
{
    public interface IPermissionedRole
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<IRolesPermissionsModel> RolesPermissions { get; set; }
    }
}