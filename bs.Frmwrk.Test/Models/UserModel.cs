using bs.Data.Interfaces.BaseEntities;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;

namespace bs.Frmwrk.Test.Models
{
    public class UserModel : IUserModel, IRoledUser, IPermissionedUser, IPersistentEntity
    {
        public virtual Guid? ConfirmationId { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string? LastIp { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual Guid? RecoveryPasswordId { get; set; }
        public virtual string? RefreshToken { get; set; }
        public virtual DateTime? RefreshTokenExpire { get; set; }
        public virtual ICollection<IRoleModel> Roles { get; set; } = new List<IRoleModel>();
        public virtual string UserName { get; set; }
        public virtual ICollection<IUsersPermissionsModel> UsersPermissions { get; set; } = new List<IUsersPermissionsModel>();
        public virtual bool? IsSystemUser { get; set; }

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
                Property(x => x.RecoveryPasswordId);
                Property(x => x.IsSystemUser);

                Property(x => x.ConfirmationId);
                Property(x => x.RefreshToken);
                Property(x => x.RefreshTokenExpire, map => map.Type<UtcDateTimeType>());
                Property(x => x.UserName, m => m.UniqueKey("UQ__UserName"));

                Bag(x => x.Roles, collectionMapping =>
                {
                    collectionMapping.Table("UsersRoles");
                    collectionMapping.Cascade(Cascade.None);
                    collectionMapping.Key(k => k.Column("UserId"));
                },
                map => map.ManyToMany(p =>
                {
                    p.Column("RoleId");
                    p.Class(typeof(RoleModel));
                    p.ForeignKey("FK__Roles_Users");
                }));

                Bag(x => x.UsersPermissions, collectionMapping =>
                {
                    collectionMapping.Inverse(true);
                    collectionMapping.Cascade(Cascade.All);
                    collectionMapping.Key(k => k.Column("UserId"));
                }, map => map.OneToMany(p =>
                {
                    p.Class(typeof(UsersPermissionsModel));
                }));
                //Bag(x => x.Permissions, collectionMapping =>
                //{
                //    collectionMapping.Table("UsersPermissions");
                //    collectionMapping.Cascade(Cascade.None);
                //    collectionMapping.Key(k => k.Column("UserId"));
                //},
                //map => map.ManyToMany(p =>
                //{
                //    p.Column("PermissionId");
                //    p.Class(typeof(PermissionModel));
                //    p.ForeignKey("FK__Permissions_Users");
                //}));
            }
        }
    }
}