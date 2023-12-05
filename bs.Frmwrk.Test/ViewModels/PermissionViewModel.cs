using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Test.ViewModels
{
    public class PermissionViewModel : IPermissionViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }
}