using GameShopAPI.Models;
using GameShopAPI.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace GameShopAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AdminU> AdminU { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
