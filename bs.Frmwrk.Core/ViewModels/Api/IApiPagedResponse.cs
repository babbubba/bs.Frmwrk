namespace bs.Frmwrk.Core.ViewModels.Api
{
    /// <summary>
    ///  The base API response for DataTables jQuery component (or any library that wrap it). See: https://datatables.net/ for componente specification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Api.IApiResponse" />
    public interface IApiPagedResponse<T> : IApiResponse
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        int Draw { get; set; }

        /// <summary>
        /// Gets or sets the records filtered.
        /// </summary>
        /// <value>
        /// The records filtered.
        /// </value>
        int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the records total.
        /// </summary>
        /// <value>
        /// The records total.
        /// </value>
        int RecordsTotal { get; set; }
    }
}