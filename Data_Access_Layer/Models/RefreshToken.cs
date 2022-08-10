using System;

namespace Repository.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
