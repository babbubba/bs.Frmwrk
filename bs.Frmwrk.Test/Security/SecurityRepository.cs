using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;

namespace bs.Frmwrk.Test.Security
{
    public class SecurityRepository : ISecurityRepository
    {
        public Task<IUserModel> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}