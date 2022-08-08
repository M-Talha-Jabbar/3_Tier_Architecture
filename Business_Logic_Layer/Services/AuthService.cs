using Repository.Contracts;
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

        public AuthService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
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

        public async Task<User> CreatePasswordHash(RegisterRequest registerModel)
        {
            var hmac = new HMACSHA512();

            var user = new User()
            {
                Username = registerModel.Username,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerModel.Password))
            };

            _userRepository.Insert(user);

            await _userRepository.SaveAsync();

            return user;
        }

        public async Task<bool> VerifyPasswordHash(LoginRequest loginModel)
        {
            var getUser = await GetUser(loginModel.Username);

            var hmac = new HMACSHA512(getUser.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginModel.Password));

            if(CompareHashes(getUser.PasswordHash, computedHash))
            {
                return true;
            }

            return false;
        }
    }
}
