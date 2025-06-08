using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
        [HttpPost("register")] // endpoint name: account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerData)
        {
                if (await UserExisits(registerData.Username))
                {
                        return BadRequest("User already exists. Choose a different user name.");
                }
                using var hmac = new HMACSHA512(); // using is needed for easier garbage collection

                var user = new AppUser
                {
                        UserName = registerData.Username.ToLower(),
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerData.Password)),
                        PasswordSalt = hmac.Key
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return new UserDto
                {
                        UserName = user.UserName,
                        Token = tokenService.CreateToken(user)
                };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
                var user = await context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower());

                if (user == null)
                {
                        return Unauthorized("Invalid username");
                }

                using var hmac = new HMACSHA512(user.PasswordSalt);

                var userDtoHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
                var correctHash = user.PasswordHash;

                for (int i = 0; i < userDtoHash.Length; i++)
                {
                        if (userDtoHash[i] != correctHash[i])
                        {
                                return Unauthorized("Incorrect password");
                        }
                }
                
                return new UserDto
                {
                        UserName = user.UserName,
                        Token = tokenService.CreateToken(user)
                };
        }

        private async Task<bool> UserExisits(string username)
        {
                return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
}
