using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;

namespace bs.Frmwrk.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public Task<IApiResponseViewModel<IUserViewModel>> AuthenticateAsync(IAuthRequestDto authRequest, string? clientIp)
        {
            throw new NotImplementedException();
        }

        public Task<IApiResponseViewModel> KeepAliveAsync(IKeepedAliveUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IApiResponseViewModel<IRefreshTokenViewModel>> RefreshAccessTokenAsync(IRefreshTokenRequestDto refreshTokenRequest)
        {
            throw new NotImplementedException();
        }
    }
}