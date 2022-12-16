//using bs.Data.Interfaces.BaseEntities;
//using bs.Frmwrk.Core.Models.Security;
//using NHibernate;
//using NHibernate.Mapping.ByCode;
//using NHibernate.Mapping.ByCode.Conformist;
//using NHibernate.Type;

//namespace bs.Frmwrk.Security.Models
//{
//    public abstract class AuditFailedLoginBaseModel : IAuditFailedLoginModel
//    {
//        public virtual string? ClientIp { get; set; }
//        public virtual DateTime EventDate { get; set; }
//        public virtual Guid Id { get; set; }
//        public virtual string UserName { get; set; }

//        public class AuditFailedLoginModelMap : ClassMapping<AuditFailedLoginBaseModel>
//        {
//            public AuditFailedLoginModelMap()
//            {
//                Table("AuditFailedLogins");

//                Id(x => x.Id, x =>
//                {
//                    x.Generator(Generators.GuidComb);
//                    x.Type(NHibernateUtil.Guid);
//                    x.Column("Id");
//                    x.UnsavedValue(Guid.Empty);
//                });

//                Property(x => x.ClientIp);
//                Property(x => x.EventDate, map => map.Type<UtcDateTimeType>());
//                Property(x => x.UserName);
//            }
//        }
//    }
//}