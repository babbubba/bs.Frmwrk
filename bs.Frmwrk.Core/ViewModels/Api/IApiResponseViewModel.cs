using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.ViewModels.Api
{
    /// <summary>
    /// The base API Response (without data)
    /// </summary>
    public interface IApiResponseViewModel
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        long? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IResponseViewModel"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        bool Success { get; set; }

        /// <summary>
        /// Gets or sets the warn message.
        /// </summary>
        /// <value>
        /// The warn message.
        /// </value>
        string? WarnMessage { get; set; }
    }

    /// <summary>
    /// The base API response (with data)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IApiResponseViewModel<T> : IApiResponseViewModel
    {
        /// <summary>
        /// Gets or sets the value field that contains the data to send to consumer.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        T Value { get; set; }
    }
}
