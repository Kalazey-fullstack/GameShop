using GameShopAPI.DTOs.Auth;
using GameShopAPI.DTOs.Auth.Admin;
using GameShopAPI.DTOs.Auth.Users;
using GameShopAPI.Helpers;
using GameShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.Annotations;

namespace GameShopAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("admin/register")]
        [Authorize(Roles = ConstantsValue.UserAdminRole)]
        [SwaggerOperation(Summary = "Register a new admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AdminRegisterResponseDTO>> AdminRegister([FromBody] AdminRegisterRequestDTO requestDTO)
        {
            try
            {
                return await _authService.AdminRegister(requestDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new AdminRegisterResponseDTO
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                });
            }
        }


        [HttpPost("user/register")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Register a new user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserRegisterResponseDTO>> UserRegister([FromBody] UserRegisterRequestDTO requestDTO)
        {
            try
            {
                return await _authService.UserRegister(requestDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new UserRegisterResponseDTO
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                });
            }
        }

        [HttpPost("admin/login")]
        [SwaggerOperation(Summary = "Log in as a admin and retrieve your JWT.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AdminLoginResponseDTO>> AdminLogin([FromBody] LoginRequestDTO loginDTO)
        {
            try
            {
                var response = await _authService.AdminLogin(loginDTO);
                if(!response.IsSuccessful)
                    return Unauthorized(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new AdminLoginResponseDTO
                {
                    IsSuccessful = false,
                    ErrorMessage = ex.Message,
                });
            }
        }

        [HttpPost("user/login")]
        [SwaggerOperation(Summary = "Log in as an user and retrieve yout JWT.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserLoginResponseDTO>> UserLogin([FromBody] LoginRequestDTO loginDTO)
        {
            try
            {
                var response = await _authService.UserLogin(loginDTO);
                if (!response.IsSucessful)
                    return Unauthorized(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new UserLoginResponseDTO
                {
                    IsSucessful = false,
                    ErrorMessage = ex.Message,
                });
            }
        }
    }
}
