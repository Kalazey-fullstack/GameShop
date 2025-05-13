using GameShopAPI.Validators;
using System.ComponentModel.DataAnnotations;

namespace GameShopAPI.DTOs.Auth.Admin
{
    public class AdminRegisterRequestDTO
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string? Email {  get; set; }
        [DataType(DataType.Password)]
        [PasswordValidator]
        public string? Password { get; set; }
    }
}
