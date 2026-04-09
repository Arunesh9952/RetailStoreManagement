using Microsoft.IdentityModel.Tokens;
using RetailShop.Data;
using RetailShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RetailShop.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly RetailDbContext context;

        public AuthService(RetailDbContext context, IConfiguration configuration)
        {
            this.context = context;
            _configuration = configuration;
        }

        // ✅ REGISTER
        public string Register(User user)
        {
            if (context.Users.Any(u => u.Username == user.Username))
            {
                return "Username already exists.";
            }

            context.Users.Add(user);
            context.SaveChanges();

            return "User registered successfully.";
        }

        // ✅ LOGIN → returns USER (not token)
        public User Login(string username, string password)
        {
            return context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // ✅ GENERATE TOKEN
        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}