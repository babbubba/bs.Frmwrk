namespace bs.Frmwrk.Core.Models.Auth
{
    /// <summary>
    /// The user's role. Any user should be member of one or more roles.
    /// </summary>
    public interface IRoleModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

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
        /// Gets or sets a value indicating whether this <see cref="IRoleModel"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }
    }
}