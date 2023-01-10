namespace bs.Frmwrk.Core.Models.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public interface IFileSystemSettings
    {
        /// <summary>
        /// Gets or sets the log te memory buffer threshold.
        /// </summary>
        /// <value>
        /// The log te memory buffer threshold.
        /// </value>
        int? MemoryBufferThresholdMb { get; set; }

        /// <summary>
        /// Gets or sets the multipart body length limit.
        /// </summary>
        /// <value>
        /// The multipart body length limit.
        /// </value>
        int? MultipartBodyLengthLimitMb { get; set; }

        /// <summary>
        /// Gets or sets the file system root path.
        /// </summary>
        /// <value>
        /// The root path.
        /// </value>
        string? RootPath { get; set; }

        /// <summary>
        /// Gets or sets the value length limit.
        /// </summary>
        /// <value>
        /// The value length limit.
        /// </value>
        int? ValueLengthLimitMb { get; set; }
    }
}