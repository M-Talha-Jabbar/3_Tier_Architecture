using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class LoginRequest
    {
        [Required][MaxLength(20)] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
