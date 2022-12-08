namespace bs.Frmwrk.Core.Models.Auth
{
    public interface IPermissionedUser
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<IPermissionModel> Permissions { get; set; }
    }
}