using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
        public string CreateToken(AppUser user)
        {
                var tokenKey = config["SecretTokenKey"] ?? throw new Exception("Cannot access secret key from appSettings.json");
                if (tokenKey.Length < 64) throw new Exception("Secret key needs to be longer than 64 characters");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

                var claims = new List<Claim>
                        {
                                new Claim(ClaimTypes.NameIdentifier, user.UserName)
                        };

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddDays(365),
                        SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
        }
}