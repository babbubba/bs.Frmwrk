namespace bs.Frmwrk.Core.ViewModels.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface ISelectListItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISelectListItem"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the extra value.
        /// </summary>
        /// <value>
        /// The extra value.
        /// </value>
        IDictionary<string, string>? ExtraValue { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }
    }
}