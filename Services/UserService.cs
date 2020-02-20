using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using TestApi.Data;
using TestApi.Models;
using TestApi.Options;
using TestApi.Contracts.V1.Responses;
using BCrypt;

namespace TestApi.Services
{
    public class UserService : IUserService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly DataContext _context;

        public UserService(JwtSettings jwtSettings, DataContext context)
        {
            _jwtSettings = jwtSettings;
            _context = context;
        }

        public async Task<IdentityResponse> LoginAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.username == username);
            if (user == null)
            {
                return new IdentityResponse
                {
                    errors = new[] { "username does not exist." }
                };
            }

            var userHasValidPassword = BCryptHelper.CheckPassword(password, user.password);
            if (!userHasValidPassword)
            {
                return new IdentityResponse
                {
                    errors = new[] { "email or password combination is wrong." }
                };
            }
            return GenerateAuthenticationResultForUser(user);
        }

        public async Task<IdentityResponse> RegisterAsync(Users user)
        {
            var checkEmail = await _context.Users
                .FirstOrDefaultAsync(x => x.email == user.email);
            if (checkEmail != null)
            {
                return new IdentityResponse
                {
                    errors = new[] { "email already exist." }
                };
            }
            var checkUsername = await _context.Users
                .FirstOrDefaultAsync(x => x.username == user.username);
            if (checkUsername != null)
            {
                return new IdentityResponse
                {
                    errors = new[] { "username already exist." }
                };
            }
            string salt = BCryptHelper.GenerateSalt(10);
            user.password = BCryptHelper.HashPassword(user.password, salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return GenerateAuthenticationResultForUser(user);
        }

        private IdentityResponse GenerateAuthenticationResultForUser(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.email),
                    new Claim("username", user.username)
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new IdentityResponse
            {
                success = true,
                token = tokenHandler.WriteToken(token),
                id = user.id,
                email = user.email,
                username = user.username
            };
        }
    }
}
