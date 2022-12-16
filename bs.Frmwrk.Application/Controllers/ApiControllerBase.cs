using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace bs.Frmwrk.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public ApiControllerBase(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        protected IPAddress ClientIp
        {
            get
            {
                return Request.HttpContext.Connection.RemoteIpAddress;
            }
        }

        protected IUserModel? CurrentUser
        {
            get
            {
                if (CurrentUserId is Guid currentUserId)
                {
                    return authRepository.GetUserByIdAsync(currentUserId).Result;
                }
                return null;
            }
        }

        protected Guid? CurrentUserId
        {
            get
            {
                if (User.Identity is not null && User.Identity.IsAuthenticated)
                {
                    var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    return userIdValue?.ToGuidN();
                }
                return null;
            }
        }
    }
}