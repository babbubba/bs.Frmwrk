using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.ViewModel.Auth
{
    public class RefreshTokenViewModels : IRefreshTokenViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpire { get; set; }
    }
}