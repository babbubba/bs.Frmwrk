using bs.Data.Interfaces.BaseEntities;
using bs.Data.Mapping;
using bs.Frmwrk.Core.Models.Auth;
using NHibernate.Mapping.ByCode;
using NHibernate;
using NHibernate.Mapping.ByCode.Conformist;

namespace bs.Frmwrk.Test.Models
{
#pragma warning disable CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.

    public class UsersPermissionsModel : IUsersPermissionsModel, IPersistentEntity
    {
        public virtual Guid Id { get; set; } 
        public virtual IUserModel User { get; set; }
        public virtual IPermissionModel Permission { get; set; }
        public virtual PermissionType Type { get; set; }

        public class Map : ClassMapping<UsersPermissionsModel>
        {
            public Map()
            {
                Table("UsersPermissions");


                Id(x => x.Id, x =>
                {
                    x.Generator(Generators.GuidComb);
                    x.Type(NHibernateUtil.Guid);
                    x.Column("Id");
                    x.UnsavedValue(Guid.Empty);
                });

                Property(x => x.Type);

                ManyToOne(x => x.User, map =>
                {
                    map.Column("UserId");
                    map.NotNullable(true);
                    map.Class(typeof(UserModel));
                    map.UniqueKey("UQ_UsersPermissions");
                });

                ManyToOne(x => x.Permission, map =>
                {
                    map.Column("PermissionId");
                    map.NotNullable(true);
                    map.Class(typeof(PermissionModel));
                    map.UniqueKey("UQ_UsersPermissions");
                });
            }
        }
    }

#pragma warning restore CS8618 // Il campo non nullable deve contenere un valore non Null all'uscita dal costruttore. Provare a dichiararlo come nullable.
}