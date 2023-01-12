using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Oops, that's not a match.")]
    [Display(Name = "Email or username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Oops, that's not a match.")]
    [Display(Name = "Password")]
    public string Password { get; set; }
}