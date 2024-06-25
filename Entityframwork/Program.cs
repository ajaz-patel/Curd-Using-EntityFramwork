

using Entityframwork.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Entityframwork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors((options) =>
            {
                options.AddPolicy("DevCors", (corsBuilder) =>
                {
                    corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
                options.AddPolicy("ProdCors", (corsBuilder) =>
                {
                    corsBuilder.WithOrigins("https://myProductionSite.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            builder.Services.AddScoped<IUserRepository,UserRepository>();

            string? tokenKeyString = builder.Configuration.GetSection("Appsettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
            tokenKeyString != null ? tokenKeyString : ""
        )
    );
            /* String? tokenString = _config.GetSection("Appsettings:TokenKey").Value;
             SymmetricSecurityKey tokenkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenString != null ? tokenString : ""));*/

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = tokenKey,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = validationParameters;
                });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("DevCors");
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseCors("ProdCors");
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();

            app.UseAuthorization();

            

            app.MapControllers();

            app.Run();
        }
    }
}
