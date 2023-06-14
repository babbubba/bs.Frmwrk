using bs.Data;
using bs.Data.Interfaces;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.WebApiDemo.Models;
using NHibernate.Linq;

namespace bs.Frmwrk.WebApiDemo.Repositories
{
    public class UsersRepository : Repository, IAuthRepository
    {
        public UsersRepository(IUnitOfWork unitOfwork) : base(unitOfwork)
        {
        }

        public Task<IRoleModel> GetRoleByIdAsync(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public async Task<IUserModel> GetUserByIdAsync(Guid userId)
        {
            return await GetByIdAsync<UserModel>(userId);
        }
    }
}