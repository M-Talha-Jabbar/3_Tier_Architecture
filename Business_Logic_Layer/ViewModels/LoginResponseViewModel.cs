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
        public string accessToken { get; }

        public LoginResponseViewModel(string username, string accessToken)
        {
            Username = username;
            this.accessToken = accessToken;
        }
    }
}
