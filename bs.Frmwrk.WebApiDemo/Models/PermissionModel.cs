using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.WebApiDemo
{
#pragma warning disable CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.

    public class PermissionModel : IPermissionModel, IPersistentEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Label { get; set; }
        public virtual bool Enabled { get; set; }

        public class Map : BsClassMapping<PermissionModel>
        {
            public Map()
            {
                Table("Permissions");
                GuidId(p => p.Id);
                PropertyUnique(p => p.Code, "UQ__Permissions");
                Property(x => x.Enabled);
                Property(x => x.Label);
            }
        }
    }

#pragma warning restore CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.
}