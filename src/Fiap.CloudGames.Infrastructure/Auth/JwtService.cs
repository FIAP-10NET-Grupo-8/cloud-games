using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fiap.CloudGames.Infrastructure.Auth;

public class JwtService(IOptions<JwtOptions> options) : IJwtService
{
	private readonly JwtOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

	public (string, DateTime) GenerateToken(Guid id, string name, string email, string role, CancellationToken ct)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, id.ToString()),
			new(ClaimTypes.Name, name),
			new(ClaimTypes.Email, email),
			new(ClaimTypes.Role, role),
            new Claim("userId", id.ToString()),
        };

		var expiresAt = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);

		var securityToken = new JwtSecurityToken(
			issuer: _options.Issuer,
			audience: _options.Audience,
			claims: claims,
			expires: expiresAt,
			signingCredentials: creds
		);

		var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

		return (token, expiresAt);
	}

	public (string, DateTime) GenerateToken(User user, CancellationToken ct) => GenerateToken(user.Id, user.Name, user.Email.Address, user.Role.ToString(), ct);
}
