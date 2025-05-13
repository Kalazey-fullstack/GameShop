using GameShopAPI.Models.Users;

namespace GameShopAPI.DTOs.Auth.Admin
{
    public class AdminRegisterResponseDTO
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage {  get; set; }
        public AdminUser? AdminUser { get; set; }
    }
}
