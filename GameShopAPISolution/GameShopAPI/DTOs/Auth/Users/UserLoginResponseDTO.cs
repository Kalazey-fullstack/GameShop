using GameShopAPI.Models.Users;

namespace GameShopAPI.DTOs.Auth.Users
{
    public class UserLoginResponseDTO
    {
        public bool IsSucessful { get; set; }
        public string? ErrorMessage { get; set; }
        public User? User { get; set; }
        public string? Token { get; set; }
    }
}
