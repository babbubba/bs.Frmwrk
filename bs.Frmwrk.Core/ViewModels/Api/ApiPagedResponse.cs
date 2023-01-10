namespace bs.Frmwrk.Core.ViewModels.Api
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Api.ApiResponse" />
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Api.IApiPagedResponse&lt;T&gt;" />
    public class ApiPagedResponse<T> : ApiResponse, IApiPagedResponse<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IEnumerable<T>? Data { get; set; }

        /// <summary>
        /// Gets or sets the draw.
        /// </summary>
        /// <value>
        /// The draw.
        /// </value>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the records filtered.
        /// </summary>
        /// <value>
        /// The records filtered.
        /// </value>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the records total.
        /// </summary>
        /// <value>
        /// The records total.
        /// </value>
        public int RecordsTotal { get; set; }
    }
}