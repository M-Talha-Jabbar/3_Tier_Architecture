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
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel registerModel)
        {
            if (await _authService.UserExists(registerModel.Username))
            {
                return BadRequest("Username is already taken.");
            }

            var res = await _authService.CreatePasswordHash(registerModel);

            return Ok(res);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginViewModel loginModel)
        {
            if (!await _authService.UserExists(loginModel.Username))
            {
                return Unauthorized("Invalid Username");
            }

            if (!await _authService.VerifyPasswordHash(loginModel))
            {
                return Unauthorized("Wrong Password");
            }

            var accessToken = _tokenService.CreateToken(loginModel); // generating & then returning access/bearer token


            // generating & then setting refresh token in HTTP Cookie
            var user = await _authService.GetUser(loginModel.Username);

            var refreshToken = await _tokenService.GenerateRefreshToken(user.Id);

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true, // cookie now will not be accessible by client-side script (such as JavaScript) or you can say in browser.
                Expires = refreshToken.TokenExpires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);


            var response = new LoginResponseViewModel(
                username: loginModel.Username,
                accessToken: accessToken
            );

            return Ok(response);
        }
    }
}
