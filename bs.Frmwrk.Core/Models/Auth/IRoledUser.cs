namespace bs.Frmwrk.Core.Models.Auth
{
    /// <summary>
    /// If user model implements this interface will be enabled role management in auth services
    /// </summary>
    public interface IRoledUser
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public IEnumerable<IRoleModel> Roles { get; set; }
    }
}
