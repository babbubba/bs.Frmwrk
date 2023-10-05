namespace bs.Frmwrk.Core.Dtos.Datatables
{
    /// <summary>
    ///
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Column" /> is orderable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if orderable; otherwise, <c>false</c>.
        /// </value>
        public bool? Orderable { get; set; } = true;

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        public Search? Search { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Column" /> is searchable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if searchable; otherwise, <c>false</c>.
        /// </value>
        public bool? Searchable { get; set; } = true;
    }
}