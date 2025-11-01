using Fiap.CloudGames.Domain.Users.Entities;

namespace Fiap.CloudGames.Domain.Users.Repositories;

public interface IUserRepository
{
	Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct);
	Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
	Task<User?> GetByEmailAsync(string email, CancellationToken ct);
	Task<User?> GetByConfirmationTokenAsync(string token, CancellationToken ct);
	Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct);
	Task<User?> GetByFirstAccessTokenAsync(string token, CancellationToken ct);

	Task AddAsync(User user, CancellationToken ct);
	Task UpdateAsync(User user, CancellationToken ct);
}
