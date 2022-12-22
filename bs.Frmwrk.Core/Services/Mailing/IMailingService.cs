using bs.Frmwrk.Core.Dtos.Mailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Services.Mailing
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailingService
    {
        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <returns></returns>
        Task SendEmailAsync(IMailMessageDto mailMessage);
    }
}
