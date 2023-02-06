namespace bs.Frmwrk.Core.Dtos.Datatables
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPageRequestDto
    {
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        Column[]? Columns { get; set; }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        /// <value>
        /// The filters.
        /// </value>
        FilterDto[]? Data { get; set; }

        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        int Draw { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        int Length { get; set; }

        /// <summary>Gets or sets the order.</summary>
        /// <value>The order.</value>
        Order[]? Order { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        int Start { get; set; }
    }
}