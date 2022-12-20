using bs.Frmwrk.Core.Dtos.Security;

namespace bs.Frmwrk.Test.Dtos
{
    public class PermissionDto : IPermissionDto
    {
        public string Id {get;set;}
        public string Code {get;set;}
        public string Name {get;set;}
        public bool Enabled {get;set;}
    }
}