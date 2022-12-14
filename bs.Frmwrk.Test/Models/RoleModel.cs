using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Test.Models
{
#pragma warning disable CS8618
    public class RoleModel : IRoleModel, IPersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Label { get; set; }
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
                Property(x => x.Label);
            }
        }
    }
#pragma warning restore CS8618
}
