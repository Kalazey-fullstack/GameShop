using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameShopAPI.Helpers;
using GameShopAPI.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using GameShopAPI.Models.Users;

namespace GameShopAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(Roles = ConstantsValue.UserAdminRole)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //GET /User
        [HttpGet]
        [SwaggerOperation(Summary = "Get Users list")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()      
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        // GET /User/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get user by Id")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return user != null ? Ok(user) : NotFound($"The user with id {id} cannot be found");
        }

        //GET /User/{email}
        [HttpGet("{email}")]
        [SwaggerOperation(Summary = "Get a user by email")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetByEmail(email);
            return user != null ? Ok(user) : NotFound($"The user with the email {email} cannot be found");
        }

        //POST /User
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                var newUser = await _userService.Create(user);
                return CreatedAtAction(nameof(GetById),
                                       new { id = newUser.Id },
                                       newUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating user : {ex.Message}");
            }
        }

        //PUT /User/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a user")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, User user)
        {
            try
            {
                var updatedUser = await _userService.Update(id, user);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"User update error : {ex.Message} ");
            }
        }

        //DELETE /user/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deleting a user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete (Guid id)
        {
            try
            {
                await _userService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user : {ex.Message}");
            }
        }
    }
}
