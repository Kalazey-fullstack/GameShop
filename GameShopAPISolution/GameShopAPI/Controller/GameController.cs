using GameShopAPI.Models;
using GameShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GameShopAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        //GET /Game
        [HttpGet]
        [SwaggerOperation(Summary = "Get games list")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var games = await _gameService.GetAllGames();
            return Ok(games);
        }

        //Get /Game/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game by Id")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var game = await _gameService.GetById(id);
                return Ok(game);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"The game with the id {id} cannot be found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get /Game/{name}
        [HttpGet("{name}")]
        [SwaggerOperation(Summary = "Get game by name")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var game = _gameService.GetByName(name);
                return Ok(game);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"The game {name} cannot be found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST /Game
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new game")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Game game)
        {
            try
            {
                var newGame = await _gameService.CreateGame(game);
                return CreatedAtAction(nameof(GetById),
                                       new { Id = newGame.Id },
                                       newGame);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating game : {ex.Message}");
            }
        }

        //PUT /Game/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a game")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Game game)
        {
            try
            {
                var updatedGame = await _gameService.UpdateGame(id, game);
                return Ok(updatedGame);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest($"Game update error: {ex.Message}");
            }
        }

        //DELETE /Game/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remove a Game")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _gameService.DeleteGame(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting game : { ex.Message}");
            }
        }
    }
}
