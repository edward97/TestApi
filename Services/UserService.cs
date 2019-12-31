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
using TestApi.Options;
using TestApi.Models;
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
                .FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return new IdentityResponse
                {
                    Errors = new[] { "username does not exitst." }
                };
            }

            var userHasValidPassword = BCryptHelper.CheckPassword(password, user.Password);
            if (!userHasValidPassword)
            {
                return new IdentityResponse
                {
                    Errors = new[] { "email or password combination is wrong" }
                };
            }

            return GenerateAuthenticationResultForUser(user);
        }

        private IdentityResponse GenerateAuthenticationResultForUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Username", user.Username)
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new IdentityResponse
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
        }
    }
}
