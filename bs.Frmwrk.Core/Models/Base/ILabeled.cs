namespace bs.Frmwrk.Core.Models.Base
{
    /// <summary>
    /// Model than can have a label and a description
    /// </summary>
    public interface ILabeled
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; set; }
    }
}