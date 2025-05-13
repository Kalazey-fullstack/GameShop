using GameShopAPI.Helpers;
using GameShopAPI.Models.Users;
using GameShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GameShopAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = ConstantsValue.UserAdminRole)]
    public class AdminUController : ControllerBase
    {
        private readonly IAdminUService _adminUService;

        public AdminUController(IAdminUService adminUService)
        {
            _adminUService = adminUService;
        }

        //GET /AdminU
        [HttpGet]
        [SwaggerOperation(Summary = "Get the list of admins")]
        [ProducesResponseType(typeof(IEnumerable<AdminU>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var admins = await _adminUService.GetAll();
            return Ok(admins);
        }

        //GET /AdminU/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get an admin with id")]
        [ProducesResponseType(typeof(AdminU), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var admin = await _adminUService.GetById(id);
            return admin != null ? Ok(admin) : NotFound($"Admin with id {id} not found");
        }

        //POST /AdminU/{id}
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new admin")]
        [ProducesResponseType(typeof(AdminU), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] AdminU admin)
        {
            try
            {
                var newAdmin = await _adminUService.Create(admin);
                return CreatedAtAction(nameof(GetById),
                                       new { id = newAdmin.Id },
                                       newAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest($"Create error for the new admin : {ex.Message}");
            }
        }

        //PUT /AdminU/{id}
        [HttpPut]
        [SwaggerOperation(Summary = "Update for an admin")]
        [ProducesResponseType(typeof(AdminU), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, AdminU admin)
        {
            try
            {
                var updatedAdmin = await _adminUService.Update(id, admin);
                return Ok(updatedAdmin);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Update error for an admin : {ex.Message}");
            }
        }

        //DELETE /AdminU/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove an admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _adminUService.DeleteById(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Remove error for the admin with id {id} : {ex.Message}");
            }
        }
    }
}
