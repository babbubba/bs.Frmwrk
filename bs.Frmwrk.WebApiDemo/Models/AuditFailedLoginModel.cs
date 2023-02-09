using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Security;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace bs.Frmwrk.WebApiDemo.Models
{
#pragma warning disable CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.

    public class AuditFailedLoginModel : IAuditFailedLoginModel
    {
        public virtual string? ClientIp { get; set; }
        public virtual DateTime EventDate { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }

        public class AuditFailedLoginModelMap : BsClassMapping<AuditFailedLoginModel>
        {
            public AuditFailedLoginModelMap()
            {
                Table("AuditFailedLogins");
                GuidId(p=>p.Id);
                Property(x => x.ClientIp);
                PropertyUtcDate(p => p.EventDate);
                Property(x => x.UserName);
            }
        }
    }

#pragma warning restore CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.
}