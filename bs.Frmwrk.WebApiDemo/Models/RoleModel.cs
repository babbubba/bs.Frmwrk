using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
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
        public virtual string Label { get; set; }
        public virtual bool Enabled { get; set; }

        public class Map : BsClassMapping<RoleModel>
        {
            public Map()
            {
                Table("Roles");
                GuidId(p => p.Id);
                Property(x => x.Code);
                Property(x => x.Enabled);
                Property(x => x.Label);
            }
        }
    }

#pragma warning restore CS8618
}