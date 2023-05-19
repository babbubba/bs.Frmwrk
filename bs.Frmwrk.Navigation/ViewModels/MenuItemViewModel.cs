using bs.Frmwrk.Core.ViewModels.Navigation;

namespace bs.Frmwrk.Navigation.ViewModels
{
    public class MenuItemViewModel : IMenuItemViewModel
    {
        public ICollection<string> AuthorizedRoles {get;set;}
        public string Code {get;set;}
        public string? CssClass {get;set;}
        public string? IconStyle {get;set;}
        public string? Id {get;set;}
        public bool? IsDefaultOpen {get;set;}
        public bool IsEnabled {get;set;}
        public bool? IsTreeView {get;set;}
        public string Label {get;set;}
        public string ParentItemCode {get;set;}
        public string ParentMenuCode {get;set;}
        public string Path {get;set;}
        public int Position {get;set;}
        public ICollection<string> RequiredPermissions {get;set;}
        public ICollection<IMenuItemViewModel>? SubItems {get;set;}
        public string? ToolTip {get;set;}
    }
}
