using bs.Frmwrk.Core.Dtos.Security;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Mapper.Profiles;

namespace bs.Frmwrk.Security.Mappings
{
    public class SecurityMapping : MappingProfile
    {
        public SecurityMapping()
        {
            CreateMapping<ICreatePermissionDto, IPermissionModel>();
        }
    }
}