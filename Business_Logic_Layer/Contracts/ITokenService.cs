using Repository.Models;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ITokenService
    {
        string CreateToken(LoginViewModel loginModel);
        string GenerateRefreshToken();
        Task<RefreshToken> GenerateRefreshToken(int UserId);
    }
}
