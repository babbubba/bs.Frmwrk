using bs.Frmwrk.Application.Controllers;
using bs.Frmwrk.Auth.Services;
using bs.Frmwrk.Core.Repositories;
using bs.Frmwrk.Core.ViewModels.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bs.Frmwrk.WebApiDemo.Controllers
{
    public class TestController : ApiControllerBase
    {
        public TestController(IAuthRepository authRepository) : base(authRepository)
        {
        }

        [HttpGet("try")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Try(string requestTest)
        {
            return Ok(new ApiResponse<string>($"This is the text you provided as parameter of this action '{requestTest}' called by the user '{CurrentUser?.UserName ?? "N/D"}'"));
        }
    }
}