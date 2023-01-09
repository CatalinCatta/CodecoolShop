using System.ComponentModel.DataAnnotations;

namespace Codecool.CodecoolShop.Models;

public class CheckoutModel
{
    [Required(ErrorMessage = "Error: Enter a first name")]
    [Display(Name = "First name")]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Error: Enter a last name.")]
    [Display(Name = "Last name")]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Error: Enter a street address.")]
    [Display(Name = "Street address 1")]
    [StringLength(100)]
    public string AddressLine1 { get; set; }

    [Display(Name = "Street address 2")]
    [StringLength(100)]
    public string AddressLine2 { get; set; }

    [Required(ErrorMessage = "Error: Please enter a phone number.")]
    [Display(Name = "Phone number")]
    [DataType(DataType.PhoneNumber)]
    [Phone]
    [StringLength(25)]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Error: Re-enter email address.")]
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [StringLength(50)]
    [EmailAddress]
    [RegularExpression(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        ErrorMessage = "Error: Re-enter email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Error: Enter a City.")]
    [StringLength(50)]
    public string City { get; set; }

    [Required(ErrorMessage = "Error: Enter a country or a region.")]
    [StringLength(50)]
    public string Country { get; set; }

    [Required(ErrorMessage = "Error: Enter a ZIP code.")]
    [Display(Name = "ZIP code")]
    [StringLength(10, MinimumLength = 4)]
    [DataType(DataType.PostalCode)]
    public string ZipCode { get; set; }
}