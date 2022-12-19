using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.WebApiDemo.Models
{
#pragma warning disable CS8618
    public class RoleModel : IRoleModel, IPersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Enabled { get; set; }

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
                Property(x => x.Name);
            }
        }
    }
#pragma warning restore CS8618
}