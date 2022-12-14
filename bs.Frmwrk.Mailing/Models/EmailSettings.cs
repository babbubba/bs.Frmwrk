using bs.Frmwrk.Core.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Mailing.Models
{
    public class EmailSettings : IEmailSettings
    {
        public string From { get; set; }
        public string FromDisplayName { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
