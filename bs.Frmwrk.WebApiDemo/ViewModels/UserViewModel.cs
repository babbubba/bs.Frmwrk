using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.WebApiDemo.ViewModels
{
    public class UserViewModel : IUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
    }
}
