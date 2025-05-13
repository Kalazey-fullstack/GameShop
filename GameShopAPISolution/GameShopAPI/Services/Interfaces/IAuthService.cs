using GameShopAPI.DTOs.Auth;
using GameShopAPI.DTOs.Auth.Admin;
using GameShopAPI.DTOs.Auth.Users;

namespace GameShopAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserRegisterResponseDTO> UserRegister(UserRegisterRequestDTO registerDTO);
        Task<UserLoginResponseDTO> UserLogin(LoginRequestDTO loginDTO);
        Task<AdminRegisterResponseDTO> AdminRegister(AdminRegisterRequestDTO registerDTO);
        Task<AdminLoginResponseDTO> AdminLogin(LoginRequestDTO loginDTO);
    }
}
