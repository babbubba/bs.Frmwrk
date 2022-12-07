namespace bs.Frmwrk.Core.Models.Auth
{
    public interface IKeepedAliveUser
    {
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public DateTime? LastPing { get; set; }
    }
}
