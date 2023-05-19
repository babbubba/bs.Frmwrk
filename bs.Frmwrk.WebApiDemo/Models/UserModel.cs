using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.WebApiDemo.Models
{
    public class UserModel : IUserModel, IRoledUser, IPermissionedUser, IPersistentEntity
    {
        public virtual Guid? ConfirmationId { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual Guid Id { get; set; }
        public bool? IsSystemUser { get; set; }
        public virtual string? LastIp { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual Guid? RecoveryPasswordId { get; set; }
        public virtual string? RefreshToken { get; set; }
        public virtual DateTime? RefreshTokenExpire { get; set; }
        public virtual ICollection<IRoleModel> Roles { get; set; }
        public virtual string UserName { get; set; }
        public virtual ICollection<IUsersPermissionsModel> UsersPermissions { get; set; }

        public class Map : BsClassMapping<UserModel>
        {
            public Map()
            {
                Table("Users");
                GuidId(p => p.Id);
                Property(x => x.Email);
                Property(x => x.Enabled);
                Property(x => x.LastIp);
                Property(x => x.ConfirmationId);
                PropertyUtcDate(x => x.LastLogin);
                Property(x => x.PasswordHash);
                Property(x => x.RecoveryPasswordId);
                Property(x => x.IsSystemUser);
                Property(x => x.RefreshToken);
                PropertyUtcDate(x => x.RefreshTokenExpire);
                PropertyUnique(x => x.UserName, "UQ__UserName");
                SetManyToMany(x => x.Roles, "UsersRoles", "UserId", "RoleId", typeof(RoleModel), false);
                SetOneToMany(x => x.UsersPermissions, "UserId", typeof(UsersPermissionsModel), true, true);
            }
        }
    }
}