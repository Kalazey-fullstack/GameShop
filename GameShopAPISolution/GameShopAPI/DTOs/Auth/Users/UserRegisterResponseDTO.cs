using GameShopAPI.Models.Users;

namespace GameShopAPI.DTOs.Auth.Users
{
    public class UserRegisterResponseDTO
    {
        public bool IsSuccessful { get; set; }
        public String? ErrorMessage { get; set; }
        public User? User { get; set; }
    }
}
