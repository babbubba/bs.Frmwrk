using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.Test.Models
{
    public class RoleModel : IRoleModel, IPermissionedRole, IPersistentEntity
    {
        public virtual string Code { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string Label { get; set; }
        public virtual ICollection<IRolesPermissionsModel> RolesPermissions { get; set; } = new List<IRolesPermissionsModel>();

        public override string ToString()
        {
            return Code;
        }

        public class Map : ClassMapping<RoleModel>
        {
            public Map()
            {
                Table("Roles");

                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.Code);
                Property(x => x.Enabled);
                Property(x => x.Label);
                Bag(x => x.RolesPermissions, collectionMapping =>
                {
                    collectionMapping.Inverse(true);
                    collectionMapping.Cascade(Cascade.All);
                    collectionMapping.Key(k => k.Column("RoleId"));
                }, map => map.OneToMany(p =>
                {
                    p.Class(typeof(RolesPermissionsModel));
                }));
            }
        }
    }
}