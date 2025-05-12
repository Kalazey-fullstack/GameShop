using System.Linq.Expressions;
using GameShopAPI.Data;
using GameShopAPI.Models.Users;
using GameShopAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameShopAPI.Repositories
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> Add(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> Get(Expression<Func<User, bool>> predicate) => await _db.Users.FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<User>> GetAll() => _db.Users;

        public async Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> predicate) => _db.Users.Where(predicate);

        public async Task<User?> GetById(Guid id) => await _db.Users.FindAsync(id);

        public async Task<User> Update(User user)
        {
            var userFromDb = await GetById(user.Id);
            if (userFromDb is null) return null;

            if (userFromDb.Email != user.Email) userFromDb.Email = user.Email;
            if (userFromDb.Password != user.Password) userFromDb.Password = user.Password;
            if (userFromDb.FirstName != user.FirstName) userFromDb.FirstName = user.FirstName;
            if (userFromDb.LastName != user.LastName) userFromDb.LastName = user.LastName;
            if (userFromDb.PhoneNumber != user.PhoneNumber) userFromDb.PhoneNumber = user.PhoneNumber;
            if (userFromDb.Address != user.Address) userFromDb.Address = user.Address;
            if (userFromDb.City != user.City) userFromDb.City = user.City;
            if (userFromDb.Region != user.Region) userFromDb.Region = user.Region;
            if (userFromDb.PostalCode != user.PostalCode) userFromDb.PostalCode = user.PostalCode;

            await _db.SaveChangesAsync();
            return userFromDb;
        }

        public async Task<bool> Delete(Guid id)
        {
            var user = await GetById(id);
            if (user == null) return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
