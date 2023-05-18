using bs.Frmwrk.Core.Dtos.Navigation;
using bs.Frmwrk.Core.Models.Navigation;
using bs.Frmwrk.Core.ViewModels.Navigation;
using bs.Frmwrk.Mapper.Profiles;


namespace bs.Frmwrk.Navigation.Mappings
{
     public class NavigationMapping : MappingProfile
    {
        public NavigationMapping()
        {
            CreateMapping<ICreateMenuDto, IMenuModel>();
                
            CreateMapping<ICreateMenuItemDto, IMenuItemModel>();

            CreateMapping<IMenuModel, IMenuViewModel>();
            CreateMapping<IMenuItemModel, IMenuItemViewModel>();
        }
    }
}
