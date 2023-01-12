using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models;

public class PaymentModel
{
    [Required(ErrorMessage = "Error: Enter Card Holder Name.")]
    [Display(Name = "Full Name")]
    [StringLength(100)]
    public string CardHolder { get; set; }

    [Required(ErrorMessage = "Error: Enter your card number.")]
    [Display(Name = "Card Number")]
    [MaxLength(16)]
    [MinLength(16)]
    public string CardNumber { get; set; }

    [Required(ErrorMessage = "Error: Enter the Expiry Month")]
    [Display(Name = "Expiry Month")]
    [MaxLength(2)]
    [MinLength(2)]
    public string ExpiryMonth { get; set; }

    [Required(ErrorMessage = "Error: Enter the Expiry Year.")]
    [Display(Name = "Expiry Year")]
    [MaxLength(2)]
    [MinLength(2)]
    public string ExpiryYear { get; set; }

    [Required(ErrorMessage = "Error: Enter the security code.")]
    [Display(Name = "CVV")]
    [MaxLength(4)]
    [MinLength(3)]
    public string Cvv { get; set; }
   
    public bool SaveData { get; set; }
}