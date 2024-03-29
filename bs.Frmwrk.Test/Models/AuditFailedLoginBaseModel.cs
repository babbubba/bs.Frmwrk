﻿using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Security;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace bs.Frmwrk.Test.Models
{
#pragma warning disable CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.

    public class AuditFailedLoginBaseModel : IAuditFailedLoginModel, IPersistentEntity
    {
        public virtual string? ClientIp { get; set; }
        public virtual DateTime EventDate { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }

        public class AuditFailedLoginModelMap : ClassMapping<AuditFailedLoginBaseModel>
        {
            public AuditFailedLoginModelMap()
            {
                Table("AuditFailedLogins");

                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.ClientIp);
                Property(x => x.EventDate, map => map.Type<UtcDateTimeType>());
                Property(x => x.UserName);
            }
        }
    }

#pragma warning restore CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.
}