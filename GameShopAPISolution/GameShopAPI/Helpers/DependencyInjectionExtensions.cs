using GameShopAPI.Data;
using GameShopAPI.Models.Users;
using GameShopAPI.Repositories;
using GameShopAPI.Repositories.Interfaces;
using GameShopAPI.Services;
using GameShopAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace GameShopAPI.Helpers
{
    public static class DependencyInjectionExtensions
    {
        public static void InjectDepencies (this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                            .AddJsonOptions(options =>
                                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.AddRepositories();
            builder.AddServices();

            builder.AddSwagger();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!));

            builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

            builder.AddAuthentification();
        }

        private static void AddSwagger (this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer ();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GameShopAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter yout Bearer token in the format **Bearer {token}** to access this API."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }


        private static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRepository<User,Guid>, UserRepository>();
            builder.Services.AddScoped<IRepository<AdminU, Guid>, AdminURepository>();
        }


        private static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<IAdminUService, AdminUService>();
        }

        private static void AddAuthentification(this WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("The JWT_SECRET_KEY environment variable is missing !");

            var key = Encoding.ASCII.GetBytes(secretKey);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}
