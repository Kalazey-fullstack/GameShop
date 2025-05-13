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
        [SwaggerOperation(Summary = "Obtenir la liste d'utilisateurs")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        // GET /USER/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obtenir un utilisateur par ID")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return user != null ? Ok(user) : NotFound($"L'utilisateur avec l'id {id} est introuvable");
        }

        //GET /User/{email}
        [HttpGet("{email}")]
        [SwaggerOperation(Summary = "Obtenir un utilisateur par l'email")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetByEmail(email);
            return user != null ? Ok(user) : NotFound($"L'utilisateur avec l'email {email} est introuvable");
        }

        //POST /User
        [HttpPost]
        [SwaggerOperation(Summary = "Créer un nouvel utilisateur")]
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
                return BadRequest($"Erreur lors de la création de l'utilisateur : {ex.Message}");
            }
        }

        //PUT /User/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Mettre à jour d'un utilisateur")]
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
                return BadRequest($"Erreur lors de la mise à jour de l'utilisateur : {ex.Message} ");
            }
        }

        //DELETE /user/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Supprimer un utilisateur")]
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
                return BadRequest($"Erreur lors de la suppression de l'utilisateur : {ex.Message}");
            }
        }
    }
}
