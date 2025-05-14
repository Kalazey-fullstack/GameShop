using GameShopAPI.Models;

namespace GameShopAPI.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGames();
        Task<Game?> GetById(int id);
        Task<Game?> GetByName(string name);
        Task<IEnumerable<Game>> GetExpansionsForGame(int baseGameId);
        Task<Game> CreateGame(Game game);
        Task<Game> UpdateGame(int id, Game game);
        Task DeleteGame(int id);
    }
}
