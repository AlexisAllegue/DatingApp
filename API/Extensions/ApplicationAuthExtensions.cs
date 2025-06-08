
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class ApplicationAuthExtensions
{
        public static IServiceCollection AddApplicationAuth(this IServiceCollection services, IConfiguration config)
        {
                //add Authentication
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                        var tokenKey = config["SecretTokenKey"] ?? throw new Exception("Secret Token Key not found");
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                                ValidateIssuer = false,
                                ValidateAudience = false
                        };
                });

                return services;
        }

}