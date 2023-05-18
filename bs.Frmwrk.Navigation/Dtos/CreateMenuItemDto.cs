using bs.Frmwrk.Core.Dtos.Navigation;

namespace bs.Frmwrk.Navigation.Dtos
{
    public class CreateMenuItemDto : ICreateMenuItemDto
    {
        public string[] AuthorizedRolesCode { get; set; }
        public string Code { get; set; }
        public string? CssClass { get; set; }
        public string? IconStyle { get; set; }
        public string? Id { get; set; }
        public bool? IsDefaultOpen { get; set; }
        public bool IsEnabled { get; set; }
        public bool? IsTreeView { get; set; }
        public string Label { get; set; }
        public string? ParentItemCode { get; set; }
        public string ParentMenuCode { get; set; }
        public string Path { get; set; }
        public int Position { get; set; }
        public string[] RequiredPermissionsCode { get; set; }
        public string? ToolTip { get; set; }
    }
}