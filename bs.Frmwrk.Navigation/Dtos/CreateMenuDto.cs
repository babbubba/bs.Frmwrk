using bs.Frmwrk.Core.Dtos.Navigation;

namespace bs.Frmwrk.Navigation.Dtos
{
    public class CreateMenuDto : ICreateMenuDto
    {
        public string Code { get; set; }
        public string? Id { get; set; }
        public bool IsEnabled {get;set;}
        public string Label {get;set;}
    }
}