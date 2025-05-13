using GameShopAPI.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameShopAPI.Models.Users
{
    public abstract class UserBase
    {
        public Guid Id { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [PasswordValidator]
        [JsonIgnore]
        public string? Password { get; set; }
    }
}
