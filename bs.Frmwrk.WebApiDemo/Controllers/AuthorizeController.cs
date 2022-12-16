using bs.Frmwrk.Application.Controllers;
using bs.Frmwrk.Core.Models.Auth;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.Services.Auth;
using bs.Frmwrk.Core.Services.Mapping;
using bs.Frmwrk.Core.ViewModels.Api;
using bs.Frmwrk.Core.ViewModels.Auth;
using bs.Frmwrk.WebApiDemo.Dtos;
using bs.Frmwrk.WebApiDemo.Models;
using bs.Frmwrk.WebApiDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bs.Frmwrk.WebApiDemo.Controllers
{
    public class AuthorizeController : ApiControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMapperService mapperService;

        public AuthorizeController(IAuthRepository authRepository, IAuthService authService, IMapperService mapperService) : base(authRepository)
        {
            this.authService = authService;
            this.mapperService = mapperService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UserViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(AuthRequestDto authRequest)
        {
            return Ok(await authService.AuthenticateAsync(authRequest, ClientIp.ToString()));
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UserViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TestMapper(AuthRequestDto authRequest)
        {
            IUserModel userModel = new UserModel
            {
                UserName = "pippo",
                Email = "pippo@paperopoli.gov.pa"
            };
            var viewModel = mapperService.Map<IUserViewModel>(userModel);
            return Ok(viewModel);
        }
    }
}