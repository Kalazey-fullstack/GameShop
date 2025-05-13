using GameShopAPI.Data;
using GameShopAPI.Models.Users;
using GameShopAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameShopAPI.Repositories
{
    public class AdminURepository : IRepository<AdminU, Guid>
    {
        private readonly AppDbContext _db;

        public AdminURepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<AdminU> Add(AdminU admin)
        {
            await _db.AddAsync(admin);
            await _db.SaveChangesAsync();
            return admin;
        }

        public async Task<AdminU?> Get(Expression<Func<AdminU, bool>> predicate) => await _db.AdminU.FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<AdminU>> GetAll() => _db.AdminU;

        public async Task<IEnumerable<AdminU>> GetAll(Expression<Func<AdminU, bool>> predicate) => _db.AdminU.Where(predicate);

        public async Task<AdminU?> GetById(Guid id) => await _db.AdminU.FindAsync(id);

        public async Task<AdminU?> Update(AdminU admin)
        {
            var adminFromDb = await GetById(admin.Id);
            if (adminFromDb is null) 
                return null;

            if (adminFromDb.Email != admin.Email)
                adminFromDb.Email = admin.Email;
            if (adminFromDb.Password != admin.Password)
                adminFromDb.Password = admin.Password;

            await _db.SaveChangesAsync();
            return adminFromDb;
        }

        public async Task<bool> Delete(Guid id)
        {
            var admin = await GetById(id);
            if (admin is null) return false;
            _db.AdminU.Remove(admin);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
