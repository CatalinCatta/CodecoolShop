using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models;

public class SignUpModel
{
    [Required(ErrorMessage = "Please enter a username.")]
    [Display(Name = "User Name")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Please enter a email address.")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [StringLength(50)]
    [EmailAddress]
    [RegularExpression(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        ErrorMessage = "Error: Re-enter email address.")]
    public string Email { get; set; }
    

    [Required(ErrorMessage = "Please enter a password.")]
    [Display(Name = "Password")]
    public string Password { get; set; }
}