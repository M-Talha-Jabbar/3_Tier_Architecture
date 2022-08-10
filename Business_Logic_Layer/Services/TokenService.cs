using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Contracts;
using Repository.Models;
using Service.Contracts;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IGenericRepository<RefreshToken> _tokenRepository;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration, IGenericRepository<RefreshToken> tokenRepository)
        {
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }
        public string CreateAccessToken(User user)
        {
            // Claims are data contained by the JWT. They are information about the user which also helps us to authorize access to a resource.
            // They could be Username, email address, role, or any other information.
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                // OR
                // new Claim(JwtRegisteredClaimNames.NameId, loginModel.Username)

                new Claim(ClaimTypes.Role, "Manager Academics")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
             );
            // If the header is fixed and the claims(i.e. in payload) are identical between two tokens, then the signature will be identical too, and you can easily get duplicated tokens.
            // So in this case the two timestamp claims "iat" and "exp" are only which can help us to avoid duplicate tokens.

            // If we comment-out the 'expires' claim and run the application and make multiple hits on api/auth/login with the same username, you will see that each time or on each hit we will get the same JWT.
            // B/c for the identical headers and identical claims there would always be an identical signature.
            // But when we have 'expires' claim in claims so on every hit on api/auth/login with the same username, we would get a different JWT.

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return jwt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RefreshToken> GenerateRefreshToken(int UserId)
        {
            var refreshToken = new RefreshToken()
            {
                Token = GenerateRefreshToken(),
                TokenCreated = DateTime.Now,
                TokenExpires = DateTime.Now.AddDays(7),
                UserId = UserId
            };

            _tokenRepository.Insert(refreshToken);

            await _tokenRepository.SaveAsync();

            return refreshToken;
        }

        // Remember: The compiler does not consider the return type while differentiating the overloaded methods. 
        // As b/c the return type alone is not sufficient for the compiler to figure out which function it has to call.
        // In overloading there are only three ways to differentiate methods of the same name: 1) different number of parameters 2) different data types of parameters & 3) different order of parameters 
    

        //public bool CompareRefreshTokens(string )
        public async Task<RefreshToken> GetRefreshToken(int UserId)
        {
            return await _tokenRepository.GetQuerable().AsNoTracking()
                                        .FirstOrDefaultAsync(t => t.UserId == UserId);
        }
    }
}
