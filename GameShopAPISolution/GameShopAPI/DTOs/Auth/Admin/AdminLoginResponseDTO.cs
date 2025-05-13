using GameShopAPI.Models.Users;

namespace GameShopAPI.DTOs.Auth.Admin
{
    public class AdminLoginResponseDTO
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public AdminU? AdminU { get; set; }
        public string? Token { get; set; }
    }
}
