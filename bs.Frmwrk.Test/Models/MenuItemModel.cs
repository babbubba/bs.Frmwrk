using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Models.Navigation;
using NHibernate.Mapping.ByCode;

namespace bs.Frmwrk.Test.Models
{
    public class MenuItemModel : IMenuItemModel, IPersistentEntity
    {
        public virtual IList<IRoleModel> AuthorizedRoles { get; set; }
        public virtual string Code { get; set; }
        public virtual string? CssClass { get; set; }
        public virtual string? IconStyle { get; set; }
        public virtual Guid Id { get; set; }
        public virtual bool? IsDefaultOpen { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual bool? IsTreeView { get; set; }
        public virtual string Label { get; set; }
        public virtual IMenuItemModel? ParentItem { get; set; }
        public virtual IMenuModel ParentMenu { get; set; }
        public virtual string Path { get; set; }
        public virtual int Position { get; set; }
        public virtual IList<IPermissionModel> RequiredPermissions { get; set; }
        public virtual ICollection<IMenuItemModel>? SubItems { get; set; }
        public virtual string? ToolTip { get; set; }

        public class Map : BsClassMapping<MenuItemModel>
        {
            public Map()
            {
                Table("MenuItems");

                GuidId(x => x.Id);

                Property(x => x.Label);
                Property(x => x.IsEnabled);
                Property(x => x.Code);
                Property(x => x.CssClass);
                Property(x => x.IconStyle);
                Property(x => x.IsDefaultOpen);
                Property(x => x.IsTreeView);
                Property(x => x.Path);
                Property(x => x.Position);
                Property(x => x.ToolTip);
                ManyToOne(x => x.ParentMenu, map =>
                {
                    map.Column("ParentMenuId");
                    map.NotNullable(true);
                    map.Class(typeof(MenuModel));
                });

                ManyToOne(x => x.ParentItem, map =>
                {
                    map.Column("ParentItemId");
                    map.Class(typeof(MenuItemModel));
                });

                Bag(x => x.AuthorizedRoles, collectionMapping =>
                {
                    collectionMapping.Table("MenuItemsAuthorizedRoles");
                    collectionMapping.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("MenuItemId"));
                },
                map => map.ManyToMany(p =>
                {
                    p.Column("RoleId");
                    p.Class(typeof(RoleModel));
                    p.ForeignKey("FK__Roles_MenuItems");
                }));

                Bag(x => x.RequiredPermissions, collectionMapping =>
                {
                    collectionMapping.Table("MenuItemsRequiredPermissions");
                    collectionMapping.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    collectionMapping.Key(k => k.Column("MenuItemId"));
                },
                map => map.ManyToMany(p =>
                {
                    p.Column("PermissionId");
                    p.Class(typeof(PermissionModel));
                    p.ForeignKey("FK__Permissions_MenuItems");
                }));

                SetOneToMany(x => x.SubItems, "ParentItemId", typeof(MenuItemModel));
            }
        }
    }
}