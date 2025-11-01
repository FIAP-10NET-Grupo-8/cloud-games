using Fiap.CloudGames.Domain.Users.Entities;

namespace Fiap.CloudGames.Domain.Users.Interfaces;

public interface IJwtService
{
	string GenerateToken(Guid id, string name, string email, string role, CancellationToken ct);
	string GenerateToken(User user, CancellationToken ct);
}
