using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // If you only want your user to be active on just one device, there’s no need to store multiple Refresh tokens. So thats why we have created only 1:1 relationship between User & RefreshToken.
        public RefreshToken RefreshToken { get; set; }
    }
}
