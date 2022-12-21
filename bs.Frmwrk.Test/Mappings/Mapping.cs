using bs.Frmwrk.Core.Dtos.Security;
using bs.Frmwrk.Core.Models.Auth;
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
            CreateMapping<IUserModel, IUserViewModel, UserViewModel>();
        }
    }
}