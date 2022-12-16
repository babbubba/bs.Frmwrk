﻿using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Security;

namespace bs.Frmwrk.Core.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecurityRepository
    {
        /// <summary>
        /// Gets the user by user name asynchronous.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<IUserModel> GetUserByUserNameAsync(string userName);
        /// <summary>
        /// Gets the instance of audit failed log model.
        /// </summary>
        /// <returns></returns>
        IAuditFailedLoginModel GetInstanceOfAuditFailedLogModel();

        //Task<IRoleModel> GetRoleByCodeAsync(string roleCode);
    }
}