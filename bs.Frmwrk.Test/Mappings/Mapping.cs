﻿using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.ViewModels.Auth;
using bs.Frmwrk.Mapper.Profiles;
using bs.Frmwrk.Test.Models;
using bs.Frmwrk.Test.ViewModels;

namespace bs.Frmwrk.Test.Mappings
{
    public class Mapping : MappingProfile
    {
        public Mapping()
        {
            CreateMapping<UserModel, IUserViewModel, UserViewModel>();
            CreateMapping<RoleModel, IRoleViewModel, RoleViewModel>();
            CreateMapping<PermissionModel, IPermissionViewModel, PermissionViewModel>();
        }
    }
}