using bs.Frmwrk.Core.ViewModels.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Navigation.ViewModels
{
    public class MenuViewModel : IMenuViewModel
    {
        public string Code {get;set;}
        public string Id {get;set;}
        public bool IsEnabled {get;set;}
        public ICollection<IMenuItemViewModel> Items {get;set;}
        public string Label {get;set;}
    }
}
