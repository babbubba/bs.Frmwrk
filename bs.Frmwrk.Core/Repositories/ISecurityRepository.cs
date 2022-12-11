using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Security;

namespace bs.Frmwrk.Core.Repositories
{
    public interface ISecurityRepository
    {
        Task<IUserModel> GetUserByUserNameAsync(string userName);
        IAuditFailedLoginModel GetInstanceOfAuditFailedLogModel();

        //Task<IRoleModel> GetRoleByCodeAsync(string roleCode);
    }
}