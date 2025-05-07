using GameShopAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace GameShopAPI.Helpers
{
    public static class DependencyInjectionExtensions
    {
        public static void InjectDepencies (this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));
        }
    }
}
