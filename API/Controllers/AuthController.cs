using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.ViewModels;
using System;
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
                return BadRequest("Username is already taken");
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

            var (loginResponse, refreshToken) = await _authService.CreateTokens(loginRequest);

            // setting refreshToken in cookie
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true, // cookie now will not be accessible by client-side script (such as JavaScript) or you can say in browser.
                Expires = refreshToken.TokenExpires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            return Ok(loginResponse);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody]int UserId)
        {
            await _authService.Logout(UserId);

            Response.Cookies.Delete("refreshToken");

            return Ok("You have been logged out");
        }

        // If access/bearer token (in our case, JWT Token) is expired, so with valid refresh token you will receive new access & refresh tokens. With this new access token you will be able maintain the user session (that means user will remain logged in).
        // But if the refresh token is also expired then the user will be logged out and have to re-login and go through the usual login process again.
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody]int UserId)
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var userRefreshToken = await _authService.GetRefreshToken(UserId);

            if (!userRefreshToken.Token.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }

            if (userRefreshToken.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Refresh Token is expired. You have to login again.");
            }

            var (newAccessToken, newRefreshToken) = await _authService.CreateTokens(UserId);

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = newRefreshToken.TokenExpires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            return Ok(newAccessToken);
        }
    }
}
