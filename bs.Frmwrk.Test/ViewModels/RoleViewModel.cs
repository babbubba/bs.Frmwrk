using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Test.ViewModels
{
    public class RoleViewModel : IRoleViewModel
    {
        public string Code { get; set; }
        public string Label { get; set; }
        public ICollection<IPermissionViewModel>? RolesPermissions { get; set; }
    }
}