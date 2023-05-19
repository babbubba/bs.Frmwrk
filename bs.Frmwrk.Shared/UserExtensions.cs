using bs.Frmwrk.Core.Globals.Auth;
using bs.Frmwrk.Core.Models.Auth;

namespace bs.Frmwrk.Shared
{
    public static class UserExtensions
    {
        public static bool IsAdmin(this IUserModel? user)
        {
            if (user is null)
            {
                return false;
            }

            if (user is IRoledUser roledUser)
            {
                return roledUser.Roles.Any(r => r.Code == RolesCodes.ADMINISTRATOR);
            }
            return true;
        }

    }
}