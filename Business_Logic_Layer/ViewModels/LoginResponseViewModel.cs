using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class LoginResponseViewModel
    {
        public string Username { get; }
        public string Token { get; }

        public LoginResponseViewModel(string username, string token)
        {
            Username = username;
            Token = token;
        }
    }
}
