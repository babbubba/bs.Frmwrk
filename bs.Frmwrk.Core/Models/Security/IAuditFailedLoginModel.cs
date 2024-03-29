﻿using bs.Frmwrk.Core.Models.Base;

namespace bs.Frmwrk.Core.Models.Security
{
    /// <summary>
    ///
    /// </summary>
    public interface IAuditFailedLoginModel : IIdentified
    {
        /// <summary>
        /// Gets or sets the client ip.
        /// </summary>
        /// <value>
        /// The client ip.
        /// </value>
        string? ClientIp { get; set; }

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>
        /// The event date.
        /// </value>
        DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; set; }
    }
}