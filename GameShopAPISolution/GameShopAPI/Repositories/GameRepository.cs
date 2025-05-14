using GameShopAPI.Data;
using GameShopAPI.Models;
using GameShopAPI.Repositories.Interfaces;
using System.Linq.Expressions;

namespace GameShopAPI.Repositories
{
    public class GameRepository : IRepository<Game, int>
    {
        private readonly AppDbContext _db;

        public GameRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Game> Add(Game game)
        {
            await _db.Games.AddAsync(game);
            await _db.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> Get(Expression<Func<Game, bool>> predicate) => await _db.Games.FindAsync(predicate);

        public async Task<IEnumerable<Game>> GetAll() => _db.Games;

        public async Task<IEnumerable<Game>> GetAll(Expression<Func<Game, bool>> predicate) => _db.Games.Where(predicate);

        public async Task<Game?> GetById(int id) => await _db.Games.FindAsync(id);

        public async Task<Game?> Update(Game game)
        {
            var gameFromDb = _db.Games.FirstOrDefault(g => g.Id == game.Id);
            if (gameFromDb is null) return null;

            if(gameFromDb.Name != game.Name) gameFromDb.Name = game.Name;
            if(gameFromDb.Description != game.Description) gameFromDb.Description = game.Description;
            if(gameFromDb.Category != game.Category) gameFromDb.Category = game.Category;
            if(gameFromDb.Price != game.Price) gameFromDb.Price = game.Price;
            if(gameFromDb.Stock != game.Stock) gameFromDb.Stock = game.Stock;
            if(gameFromDb.Publisher != game.Publisher) gameFromDb.Publisher = game.Publisher;
            if(gameFromDb.IsExpansion != game.IsExpansion) gameFromDb.IsExpansion = game.IsExpansion;
            if(gameFromDb.BaseGameId != game.BaseGameId) gameFromDb.BaseGameId = game.BaseGameId;
            if(gameFromDb.BaseGame != game.BaseGame) gameFromDb.BaseGame = game.BaseGame;
            if(gameFromDb.Expansions != game.Expansions) gameFromDb.Expansions = game.Expansions;

            await _db.SaveChangesAsync();
            return game;

        }

        public async Task<bool> Delete(int id)
        {
            var game = await GetById(id);
            if (game is null) return false;

            _db.Games.Remove(game);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
