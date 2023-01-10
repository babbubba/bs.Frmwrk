using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.WebApiDemo.ViewModels
{
#pragma warning disable CS8618
    public class UserViewModel : IUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public DateTime? LastLogin { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
    }
#pragma warning restore CS8618
}