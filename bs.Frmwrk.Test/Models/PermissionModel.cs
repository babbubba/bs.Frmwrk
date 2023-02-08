using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.Test.Models
{
#pragma warning disable CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.

    public class PermissionModel : IPermissionModel, IPersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Enabled { get; set; }

        public class Map : ClassMapping<PermissionModel>
        {
            public Map()
            {
                Table("Permissions");

                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.Code, map => map.UniqueKey("UQ__Permissions"));
                Property(x => x.Enabled);
                Property(x => x.Name);
            }
        }
    }

#pragma warning restore CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.
}