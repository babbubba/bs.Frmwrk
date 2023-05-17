namespace bs.Frmwrk.Core.ViewModels.Auth
{
    public interface IPermissionedUserViewModel
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<IUsersPermissionsViewModel> UsersPermissions { get; set; }
    }

}