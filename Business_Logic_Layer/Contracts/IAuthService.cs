using Repository.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IAuthService
    {
        bool CompareHashes(byte[] hash, byte[] computedHash);
        Task<User> CreatePasswordHash(RegisterRequest registerModel);
        Task<RefreshToken> GetRefreshToken(int UserId);
        Task<User> GetUser(string username);
        Task<(LoginResponse, RefreshToken)> CreateTokens(LoginRequest loginRequest);
        Task<(string, RefreshToken)> CreateTokens(int UserId);
        Task<bool> UserExists(string username);
        Task<bool> VerifyPasswordHash(LoginRequest loginModel);
        Task Logout(int UserId);
    }
}
