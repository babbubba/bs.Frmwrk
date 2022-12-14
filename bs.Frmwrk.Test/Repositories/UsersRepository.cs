﻿using bs.Data;
using bs.Data.Interfaces;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Security;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Test.Models;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Test.Repositories
{
    public class UsersRepository : Repository,  IAuthRepository, ISecurityRepository
    {
        public UsersRepository(IUnitOfWork unitOfwork) : base(unitOfwork)
        {
        }

        public Task CreateUserAsync(IUserModel userModel)
        {
            throw new NotImplementedException();
        }

        public IAuditFailedLoginModel GetInstanceOfAuditFailedLogModel()
        {
            return new AuditFailedLoginBaseModel();
        }

        public Task<IRoleModel> GetRoleByIdAsync(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IUserModel> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IUserModel> GetUserByUserNameAsync(string userName)
        {
            return await Query<UserModel>().SingleOrDefaultAsync(u => u.UserName == userName);
        }
    }
}