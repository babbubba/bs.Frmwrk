using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.ViewModels.Auth;
using bs.Frmwrk.Mapper.Profiles;
using bs.Frmwrk.WebApiDemo.ViewModels;

namespace bs.Frmwrk.WebApiDemo.Mappings
{
    public class Mapping : MappingProfile
    {
        public Mapping()
        {
            CreateMapping<IUserModel, IUserViewModel, UserViewModel>();
        }
    }
}