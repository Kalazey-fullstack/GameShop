using GameShopAPI.Validators;
using System.ComponentModel.DataAnnotations;

namespace GameShopAPI.DTOs.Auth.Users
{
    public class UserRegisterRequestDTO
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage =  "Email is required")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [PasswordValidator]
        public string? Password { get; set; }
        [Required(ErrorMessage = "FirstName is required")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Firstname must start with an Uppercase Letter !")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        [RegularExpression(@"^[A-Z\-]*", ErrorMessage = "LastName must be only Uppercase !")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [RegularExpression("^[0-9]{1,5}(?:[ ]?[A-Za-zÀ-ÿ0-9'’\\-\\.]+)+$", ErrorMessage = "Address is invalid")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[A-Za-zÀ-ÿ'’\\- ]{2,50}$", ErrorMessage = "City is invalid")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Region is required")]
        [RegularExpression("^[A-Za-zÀ-ÿ'’\\- ]{2,50}$", ErrorMessage = "Region is invalid")]
        public string? Region { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        [RegularExpression("^\\d{4,5}$", ErrorMessage = "Invalid Postal Code")]
        public string? PostalCode { get; set; }
    }
}
