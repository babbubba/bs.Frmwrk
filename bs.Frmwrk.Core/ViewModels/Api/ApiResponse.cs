namespace bs.Frmwrk.Core.ViewModels.Api
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Api.IApiResponse" />
    public class ApiResponse : IApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        public ApiResponse()
        {
            Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="errorMessage">The error message.</param>
        public ApiResponse(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public long? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IApiResponse" /> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the warn message.
        /// </summary>
        /// <value>
        /// The warn message.
        /// </value>
        public string? WarnMessage { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="bs.Frmwrk.Core.ViewModels.Api.IApiResponse" />
    public class ApiResponse<T> : ApiResponse, IApiResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        public ApiResponse() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ApiResponse(T? value) : base()
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="errorMessage">The error message.</param>
        public ApiResponse(bool success, string errorMessage) : base(success, errorMessage) { }

        /// <summary>
        /// Gets or sets the value field that contains the data to send to consumer.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T? Value { get; set; }
    }
}