namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface ICreateRoleDto
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the permissions code that are associated to the role.
        /// </summary>
        /// <value>
        /// The permissions code.
        /// </value>
        public string[]? PermissionsCode { get; set; }
    }
}