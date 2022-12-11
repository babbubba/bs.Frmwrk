using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;

namespace bs.Frmwrk.Test.Auth
{
    public class AuthRepository : IAuthRepository
    {
        public Task CreateUserAsync(IUserModel userModel)
        {
            throw new NotImplementedException();
        }

        public Task<IRoleModel> GetRoleByIdAsync(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IUserModel> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IUserModel> GetUserByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}