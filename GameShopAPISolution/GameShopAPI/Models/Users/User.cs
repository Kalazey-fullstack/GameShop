using System.ComponentModel.DataAnnotations;

namespace GameShopAPI.Models.Users
{
    public class User : UserBase
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? City { get; set; }
        public string? Region { get; set; }
        [Required]
        public string? PostalCode { get; set; }
    }
}
