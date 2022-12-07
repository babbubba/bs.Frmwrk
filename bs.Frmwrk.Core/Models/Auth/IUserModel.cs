using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Models.Auth
{
    public interface IUserModel
    {
         Guid Id { get; set; }
         string UserName { get; set; }
         string Email { get; set; }
         string PasswordHash { get; set; }
         bool Enabled { get; set; }
         DateTime? LastLogin { get; set; }
        string? RefreshToken { get; set; }
        DateTime? RefreshTokenExpire { get; set; }
        string? LastIp { get; set; }
    }
}
