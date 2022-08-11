using System;

namespace Repository.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        // If you only want your user to be active on just one device, there’s no need to store multiple Refresh tokens. So thats why we have created only 1:1 relationship between User & RefreshToken.
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
