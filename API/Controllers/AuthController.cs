using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.ViewModels;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterRequest registerRequest)
        {
            if (await _authService.UserExists(registerRequest.Username))
            {
                return BadRequest("Username is already taken.");
            }

            var res = await _authService.CreatePasswordHash(registerRequest);

            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginRequest loginRequest)
        {
            if (!await _authService.UserExists(loginRequest.Username))
            {
                return Unauthorized("Invalid Username");
            }

            if (!await _authService.VerifyPasswordHash(loginRequest))
            {
                return Unauthorized("Wrong Password");
            }

            var(loginResponse, refreshToken) = await _authService.Login(loginRequest);

            // setting refreshToken in cookie
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true, // cookie now will not be accessible by client-side script (such as JavaScript) or you can say in browser.
                Expires = refreshToken.TokenExpires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            return Ok(loginResponse);
        }
    }
}
