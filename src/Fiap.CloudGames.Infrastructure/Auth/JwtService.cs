using Fiap.CloudGames.Domain.Users.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fiap.CloudGames.Infrastructure.Auth;

public class JwtService(JwtOptions options)
{
	private readonly JwtOptions _options = options;

	public string GenerateToken(Guid id, string name, string email, string role)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, id.ToString()),
			new(ClaimTypes.Name, name),
			new(ClaimTypes.Email, email),
			new(ClaimTypes.Role, role),
		};

		var token = new JwtSecurityToken(
			issuer: _options.Issuer,
			audience: _options.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateToken(User user) => GenerateToken(user.Id, user.Name, user.Email.Address, user.Role.ToString());
}
