using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using cloud_games_back.Services.Interfaces;
using cloud_games_back.Entities;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace cloud_games_back.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string?> LoginAsync(string email, string senha)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPassword(senha, user.SenhaHash))
                return null;

            return GenerateJwtToken(user);
        }

        public async Task SeedUserAsync()
        {
            if (!await _context.Usuarios.AnyAsync())
            {
                var user = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = "Admin",
                    Email = "admin@bananaltda.com",
                    SenhaHash = HashPassword("123456")
                };
                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();
            }
        }

        private string GenerateJwtToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Nome)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash) =>
            HashPassword(password) == hash;
    }
}
