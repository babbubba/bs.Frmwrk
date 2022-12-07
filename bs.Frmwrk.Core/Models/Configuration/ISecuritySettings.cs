using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Models.Configuration
{
    public interface ISecuritySettings
    {
        string? ValidTokenIssuer { get; set; }
        string? ValidTokenAudience { get; set; }
        bool? ValidateIssuer { get; set; }
        bool? ValidateAudience { get; set; }
        int? JwtRefreshTokenValidityMinutes { get; set; }
        int? JwtTokenValidityMinutes { get; set; }
        string? Secret { get; set; }
    }
}
