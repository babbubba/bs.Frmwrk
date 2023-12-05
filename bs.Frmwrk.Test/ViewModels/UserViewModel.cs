using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Test.ViewModels
{
    public class UserViewModel : IUserViewModel
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public string Id { get; set; }
        public DateTime? LastLogin { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
        public IRoleViewModel[] Roles { get; set; }
        public string UserName { get; set; }
    }
}