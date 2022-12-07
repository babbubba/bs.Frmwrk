using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<IUserModel> GetUserByUserNameAsync(string userName);

        Task<IUserModel> GetUserByIdAsync(Guid userId);

        Task<IRoleModel> GetRoleByIdAsync(Guid roleId);

        Task CreateUserAsync(IUserModel userModel);
    }
}