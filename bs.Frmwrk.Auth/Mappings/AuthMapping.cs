using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Mapper.Profiles;

namespace bs.Frmwrk.Auth.Mappings
{
    public class AuthMapping : MappingProfile
    {
        public AuthMapping()
        {
            CreateMapping<ICreateUserDto, IUserModel>();
            CreateMapping<ICreateRoleDto, IRoleModel>();
        }
    }
}