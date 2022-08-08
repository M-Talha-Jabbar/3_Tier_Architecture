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
        Task<User> GetUser(string username);
        Task<(LoginResponse, RefreshToken)> Login(LoginRequest loginRequest);
        Task<bool> UserExists(string username);
        Task<bool> VerifyPasswordHash(LoginRequest loginModel);
    }
}
