namespace bs.Frmwrk.Core.Dtos.Auth
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityEventDto
    {
        /// <summary>
        /// Gets the client ip.
        /// </summary>
        /// <value>
        /// The client ip.
        /// </value>
        string? ClientIp { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        string Message { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ISecurityEventDto"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        SecurityEventSeverity Severity { get; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string? UserName { get; }
    }

    /// <summary>
    ///
    /// </summary>
    public enum SecurityEventSeverity
    {
        /// <summary>
        /// The verbose
        /// </summary>
        Verbose = 0,

        /// <summary>
        /// The information
        /// </summary>
        Info = 10,

        /// <summary>
        /// The warning
        /// </summary>
        Warning = 20,

        /// <summary>
        /// The error
        /// </summary>
        Danger = 30,

        /// <summary>
        /// The critical
        /// </summary>
        Critical = 40
    }
}