using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

                services.AddControllers();
                services.AddDbContext<DataContext>(opt =>
                {
                        opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                });
                services.AddCors();
                //add TokenService
                services.AddScoped<ITokenService, TokenService>(); //once per http request
                return services;
        }
}