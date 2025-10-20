using Fiap.CloudGames.Domain.Users.Entities;

namespace Fiap.CloudGames.Domain.Users.Repositories;

public interface IUserRepository
{
	Task<IReadOnlyList<User>> GetAllAsync();
	Task<User?> GetByIdAsync(Guid id);
	Task<User?> GetByEmailAsync(string email);
	Task<User?> GetByConfirmationTokenAsync(string token);
	Task<User?> GetByPasswordResetTokenAsync(string token);
	Task<User?> GetByFirstAccessTokenAsync(string token);

	Task AddAsync(User user);
	Task UpdateAsync(User user);
}
