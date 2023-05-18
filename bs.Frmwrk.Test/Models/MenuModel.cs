using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Navigation;

namespace bs.Frmwrk.Test.Models
{
    public class MenuModel : IMenuModel, IPersistentEntity
    {
        public virtual string Code { get; set; }
        public virtual Guid Id { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual ICollection<IMenuItemModel> Items { get; set; }
        public virtual string Label { get; set; }

        public class Map : BsClassMapping<MenuModel>
        {
            public Map()
            {
                Table("Menus");

                GuidId(x => x.Id);

                Property(x => x.Label);
                Property(x => x.IsEnabled);
                Property(x => x.Code);

                SetOneToMany(x => x.Items, "MenuId", typeof(MenuItemModel));
            }
        }
    }
}