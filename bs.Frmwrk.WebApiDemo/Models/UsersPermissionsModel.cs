using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.WebApiDemo.Models
{
#pragma warning disable CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.

    public class UsersPermissionsModel : IUsersPermissionsModel, IPersistentEntity
    {
        public virtual IUserModel User { get; set; }
        public virtual IPermissionModel Permission { get; set; }
        public virtual PermissionType Type { get; set; }

        public class Map : BsClassMapping<UsersPermissionsModel>
        {
            public Map()
            {
                Table("UsersPermissions");
                Property(x => x.Type);
                SetManyToOne(p => p.User, "UserId", "FK__UsersPermissions_Users", mto =>
                {
                    mto.NotNullable(true);
                    mto.Class(typeof(UserModel));
                    mto.UniqueKey("UQ_UsersPermissions");
                });

                //ManyToOne(x => x.User, map =>
                //{
                //    map.Column("UserId");
                //    map.NotNullable(true);
                //    map.Class(typeof(UserModel));
                //    map.UniqueKey("UQ_UsersPermissions");
                //});



                SetManyToOne(p => p.Permission, "PermissionId", "FK__UsersPermissions_Permissions", mto =>
                {
                    mto.NotNullable(true);
                    mto.Class(typeof(PermissionModel));
                    mto.UniqueKey("UQ_UsersPermissions");
                });

                //ManyToOne(x => x.Permission, map =>
                //{
                //    map.Column("PermissionId");
                //    map.NotNullable(true);
                //    map.Class(typeof(PermissionModel));
                //    map.UniqueKey("UQ_UsersPermissions");
                //});
            }
        }
    }

#pragma warning restore CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.
}