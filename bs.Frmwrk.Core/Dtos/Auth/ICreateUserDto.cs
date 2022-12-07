using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Dtos.Auth
{
    public  interface ICreateUserDto
    {
        string UserName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool Enabled { get; set; }

        string[]? RolesIds { get; set; }
    }
}
