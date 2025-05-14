using GameShopAPI.Models;
using GameShopAPI.Repositories;
using GameShopAPI.Repositories.Interfaces;
using GameShopAPI.Services.Interfaces;

namespace GameShopAPI.Services
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game,int> _gameRepository;
        public GameService(IRepository<Game, int> gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> CreateGame(Game game)
        {
            try
            {
                return await _gameRepository.Add(game);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error adding game {game.Id} - {game.Name} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public Task<IEnumerable<Game>> GetAllGames() => _gameRepository.GetAll();

        public Task<Game?> GetById(int id) => _gameRepository.GetById(id);

        public Task<IEnumerable<Game>> GetExpansionsForGame(int baseGameId) => _gameRepository.GetAll(g => g.BaseGameId == baseGameId);

        public async Task<Game> UpdateGame(int id, Game game)
        {
            try
            {
                game.Id = id;
                return await _gameRepository.Update(game) ?? throw new KeyNotFoundException($"The game with id {id} cannot be found.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error modifying game with id {id} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        public async Task DeleteGame(int id)
        {
            try
            {
                if(!await  _gameRepository.Delete(id))
                    throw new KeyNotFoundException($"The game with id {id} cannot be found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting game with id {id} : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
