namespace bs.Frmwrk.Core.Exceptions
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class BsException : Exception
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public long? ErrorCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsException"/> class.
        /// </summary>
        public BsException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public BsException(long errorCode) : base()
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public BsException(string message, Exception ex) : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        public BsException(long errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public BsException(long errorCode, string message, Exception ex) : base(message, ex)
        {
            ErrorCode = errorCode;
        }
    }
}