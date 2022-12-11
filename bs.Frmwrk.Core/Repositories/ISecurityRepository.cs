using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Core.Repositories
{
    public interface ISecurityRepository
    {
        Task<IUserModel> GetUserByUserNameAsync(string userName);

        //Task<IRoleModel> GetRoleByCodeAsync(string roleCode);
    }
}