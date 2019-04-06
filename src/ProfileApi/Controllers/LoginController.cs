using System.Collections.Generic;
using ProfileApi.Models;
using ProfileApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProfileApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]User user)
        {
            var authUserToken = _loginService.Login(user.Username, user.Password);

            if (authUserToken == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(authUserToken);
        }
    }
}