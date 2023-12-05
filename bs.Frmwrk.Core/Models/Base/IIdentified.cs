namespace bs.Frmwrk.Core.Models.Base
{
    /// <summary>
    /// model that can be identified by a Guid
    /// </summary>
    public interface IIdentified
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; set; }
    }
}