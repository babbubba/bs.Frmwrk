using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.Test.Models
{
    public class RolesPermissionsModel : IRolesPermissionsModel, IPersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual IPermissionModel Permission { get; set; }
        public virtual PermissionType Type { get; set; }
        public virtual IPermissionedRole Role { get; set; }

        public class Map : ClassMapping<RolesPermissionsModel>
        {
            public Map()
            {
                Table("RolesPermissions");

                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.Type);

                ManyToOne(x => x.Role, map =>
                {
                    map.Column("RoleId");
                    map.NotNullable(true);
                    map.Class(typeof(RoleModel));
                    map.UniqueKey("UQ_RolesPermissions");
                });

                ManyToOne(x => x.Permission, map =>
                {
                    map.Column("PermissionId");
                    map.NotNullable(true);
                    map.Class(typeof(PermissionModel));
                    map.UniqueKey("UQ_RolesPermissions");
                });
            }
        }
    }

#pragma warning restore CS8618
}