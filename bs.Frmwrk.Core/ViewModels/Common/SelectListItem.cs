using bs.Frmwrk.Core.Exceptions;

namespace bs.Frmwrk.Core.ViewModels.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Common.ISelectListItem" />
    public class SelectListItem : ISelectListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListItem"/> class.
        /// </summary>
        public SelectListItem()
        {
            Enabled = true;
            Id = Guid.Empty.ToString();
            Label = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="label">The label.</param>
        public SelectListItem(string id, string? label)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BsException(2212200821, $"Il campo '{nameof(id)}' non può essere vuoto.");
            }

            Id = id;
            Label = label ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public SelectListItem(string id, string? label, bool enabled) : this(id, label)
        {
            Enabled = enabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectListItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="label">The label.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <param name="extraValue">The extra value.</param>
        public SelectListItem(string id, string? label, bool enabled, IDictionary<string, string>? extraValue) : this(id, label, enabled)
        {
            ExtraValue = extraValue;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ISelectListItem" /> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the extra value.
        /// </summary>
        /// <value>
        /// The extra value.
        /// </value>
        public IDictionary<string, string>? ExtraValue { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; } = string.Empty;
    }
}