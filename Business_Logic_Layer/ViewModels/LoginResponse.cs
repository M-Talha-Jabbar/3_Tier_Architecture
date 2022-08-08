using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class LoginResponse
    {
        public string Username { get; }
        public string accessToken { get; }

        public LoginResponse(string username, string accessToken)
        {
            Username = username;
            this.accessToken = accessToken;
        }
    }
}
