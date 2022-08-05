using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class RegisterViewModel
    {
        [Required][MaxLength(20)] public string Username { get; set; }
        [Required][Compare("ConfirmPassword", ErrorMessage = "Passwords don't match")] public string Password { get; set; }
        [Required]  public string ConfirmPassword { get; set; }
    }
}
