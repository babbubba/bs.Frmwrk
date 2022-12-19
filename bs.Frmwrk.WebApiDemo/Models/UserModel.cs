using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace bs.Frmwrk.WebApiDemo.Models
{
#pragma warning disable CS8618
    public class UserModel : IUserModel, IRoledUser, IPersistentEntity
    {
        public virtual string Email { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string? LastIp { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string? RefreshToken { get; set; }
        public virtual DateTime? RefreshTokenExpire { get; set; }
        public virtual string UserName { get; set; }
        public virtual ICollection<IRoleModel> Roles { get; set; }

        public class Map : ClassMapping<UserModel>
        {
            public Map()
            {
                Table("Users");

                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.Email);
                Property(x => x.Enabled);
                Property(x => x.LastIp);
                Property(x => x.LastLogin, map => map.Type<UtcDateTimeType>());
                Property(x => x.PasswordHash);
                Property(x => x.RefreshToken);
                Property(x => x.RefreshTokenExpire, map => map.Type<UtcDateTimeType>());
                Property(x => x.UserName, m => m.UniqueKey("UQ__UserName"));

                Bag(x => x.Roles, collectionMapping =>
                {
                    collectionMapping.Table("UsersRoles");
                    collectionMapping.Cascade(Cascade.None);
                    collectionMapping.Key(k => k.Column("UserId"));
                    //collectionMapping.Lazy(CollectionLazy.NoLazy);
                    //collectionMapping.Fetch(CollectionFetchMode.Join);
                },
                map => map.ManyToMany(p =>
                {
                    p.Column("RoleId");
                    p.Class(typeof(RoleModel));
                    p.ForeignKey("FK__Roles_Users");
                }));
            }
        }
    }
#pragma warning restore CS8618
}