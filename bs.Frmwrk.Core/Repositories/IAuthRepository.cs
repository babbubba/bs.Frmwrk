using bs.Frmwrk.Core.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<IUserModel> GetUserByUserNameAsync(string userName);
    }
}
