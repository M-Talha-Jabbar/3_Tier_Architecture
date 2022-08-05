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

            var response = new LoginResponseViewModel(
                username: loginModel.Username,
                token: _tokenService.CreateToken(loginModel)
            );

            return Ok(response);
        }
    }
}
