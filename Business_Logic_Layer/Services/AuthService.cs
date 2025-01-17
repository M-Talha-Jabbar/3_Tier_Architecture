﻿using Repository.Contracts;
using Repository.Models;
using Service.Contracts;
using Service.ViewModels;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IGenericRepository<User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _userRepository.GetQuerable().AnyAsync(u => u.Username == username);
        }

        public async Task<User> GetUser(string username)
        {
            return await _userRepository.GetQuerable()
                                    .FirstOrDefaultAsync(u => u.Username == username);
        }

        public bool CompareHashes(byte[] hash, byte[] computedHash)
        {
            return hash.SequenceEqual(computedHash);
        }

        public async Task<User> CreatePasswordHash(RegisterRequest registerRquest)
        {
            var hmac = new HMACSHA512();

            var user = new User()
            {
                Username = registerRquest.Username,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRquest.Password))
            };

            _userRepository.Insert(user);

            await _userRepository.SaveAsync();

            return user;
        }

        public async Task<bool> VerifyPasswordHash(LoginRequest loginRequest)
        {
            var getUser = await GetUser(loginRequest.Username);

            var hmac = new HMACSHA512(getUser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            if(CompareHashes(getUser.PasswordHash, computedHash))
            {
                return true;
            }

            return false;
        }

        public async Task<(LoginResponse, RefreshToken)> CreateTokens(LoginRequest loginRequest)
        {
            var getUser = await GetUser(loginRequest.Username);

            var accessToken = _tokenService.CreateAccessToken(getUser); // generating access/bearer token (in our case, JWT)

            var refreshToken = await _tokenService.GenerateRefreshToken(getUser.Id); // generating refresh token and it will be set in HTTP Cookie

            var loginResponse = new LoginResponse(
                userId: getUser.Id,
                username: getUser.Username,
                accessToken: accessToken
            );

            return (loginResponse, refreshToken);
        }

        public async Task<(string, RefreshToken)> CreateTokens(int UserId)
        {
            var getUser = await _userRepository.GetByIdAsync(UserId);

            var accessToken = _tokenService.CreateAccessToken(getUser); // generating access/bearer token (in our case, JWT)

            var refreshToken = await _tokenService.GenerateRefreshToken(getUser.Id); // generating refresh token and it will be set in HTTP Cookie

            return (accessToken, refreshToken);
        }

        public async Task<RefreshToken> GetRefreshToken(int UserId)
        {
            var refreshToken = await _tokenService.GetRefreshToken(UserId);

            return refreshToken;
        }
    }
}
