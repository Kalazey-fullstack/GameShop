using GameShopAPI.DTOs.Auth;
using GameShopAPI.DTOs.Auth.Admin;
using GameShopAPI.DTOs.Auth.Users;
using GameShopAPI.Helpers;
using GameShopAPI.Models.Users;
using GameShopAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameShopAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IAdminUService _adminUService;
        private readonly ILogger<AuthService> _logger;
        private readonly AppSettings _appSettings;
        private readonly Encryptor _encryptor;

        public AuthService(IUserService userService, 
                           IAdminUService adminUService, 
                           ILogger<AuthService> logger, 
                           IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _adminUService = adminUService;
            _logger = logger;
            _appSettings = appSettings.Value;
            _encryptor = new Encryptor();

            logger.LogInformation("Auth service created");
        }

        public async Task<AdminLoginResponseDTO> AdminLogin(LoginRequestDTO loginDTO)
        {
            try
            {
                var admin = await _adminUService.GetByEmail(loginDTO.Email);

                if (admin == null)
                    throw new KeyNotFoundException("Invalid Authentification !");

                var (verified, needSUpgrade) = _encryptor.Check(admin.Password!, loginDTO.Password!);

                if (!verified)
                    throw new UnauthorizedAccessException("Invalid Authentification !");

                if (needSUpgrade)
                {
                    admin.Password = loginDTO.Password;
                    await _adminUService.Update(admin.Id, admin);
                }

                string token = CreateJwt(ConstantsValue.UserAdminRole, admin.Id.ToString());

                return new AdminLoginResponseDTO
                {
                    IsSuccessful = true,
                    Token = token,
                    AdminU = admin,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Connection error for the user {loginDTO.Email} : {ex.Message}");
                throw;
            }
        }

        public async Task<AdminRegisterResponseDTO> AdminRegister(AdminRegisterRequestDTO registerDTO)
        {
            try
            {
                if (await _adminUService.GetByEmail(registerDTO.Email!) is not null)
                    throw new InvalidOperationException("Email already exist !");

                var admin = new AdminU
                {
                    Email = registerDTO.Email,
                    Password = registerDTO.Password,
                };

                admin = await _adminUService.Create(admin);

                return new AdminRegisterResponseDTO { IsSuccessful = true, AdminU = admin };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Register error for the user {registerDTO.Email} : {ex.Message}");
                throw;
            }
        }

        public Task<UserLoginResponseDTO> UserLogin(LoginRequestDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRegisterResponseDTO> UserRegister(UserRegisterRequestDTO registerDTO)
        {
            _logger.LogInformation("UserRegister called");
            try
            {
                if (await _userService.GetByEmail(registerDTO.Email!) is not null)
                    throw new InvalidOperationException("Email already exist !");

                var user = new User
                {
                    Email = registerDTO.Email,
                    Password = registerDTO.Password,
                    FirstName = registerDTO.FirstName,
                    LastName = registerDTO.LastName,
                    PhoneNumber = registerDTO.PhoneNumber,
                    Address = registerDTO.Address,
                    City = registerDTO.City,
                    Region = registerDTO.Region,
                    PostalCode = registerDTO.PostalCode,
                };

                user = await _userService.Create(user);

                return new UserRegisterResponseDTO { IsSuccessful = true, User = user }; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Register error for the user {registerDTO.Email} : {ex.Message}");
                throw;
            }
        }




        private string CreateJwt(string role, string subjectId)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Role, role),
                new (JwtRegisteredClaimNames.Sub, subjectId),
            };

            var securityKey = _appSettings.SecretKey;

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_appSettings.TokenExpirationDays),
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
