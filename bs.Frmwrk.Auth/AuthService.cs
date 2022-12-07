using AutoMapper;
using bs.Data.Interfaces;
using bs.Frmwrk.Base;
using bs.Frmwrk.Core.Dtos.Auth;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Locale;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;
using Microsoft.Extensions.Logging;

namespace bs.Frmwrk.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        protected readonly IAuthRepository authRepository;

        public AuthService(ILogger<AuthService> logger, ITranslateService translateService, IMapper mapper, IUnitOfWork unitOfWork,
            IAuthRepository authRepository) : base(logger, translateService, mapper, unitOfWork)
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