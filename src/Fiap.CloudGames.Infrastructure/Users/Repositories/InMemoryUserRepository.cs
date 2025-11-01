using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Repositories;

namespace Fiap.CloudGames.Infrastructure.Users.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private static readonly List<User> _users = [];

	public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
	{
		return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
	}

	public Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct)
	{
		return Task.FromResult((IReadOnlyList<User>)[.. _users]);
	}

	public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
	{
		return Task.FromResult(_users.FirstOrDefault(u => string.Equals(u.Email.Address, email, StringComparison.OrdinalIgnoreCase)));
	}

	public Task<User?> GetByConfirmationTokenAsync(string token, CancellationToken ct)
	{
		return Task.FromResult(_users.FirstOrDefault(u => u.ConfirmationToken == token));
	}

	public Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct)
	{
		return Task.FromResult(_users.FirstOrDefault(u => u.PasswordResetToken == token));
	}

	public Task<User?> GetByFirstAccessTokenAsync(string token, CancellationToken ct)
	{
		return Task.FromResult(_users.FirstOrDefault(u => u.FirstAccessToken == token));
	}

	public Task AddAsync(User user, CancellationToken ct)
	{
		_users.Add(user);
		return Task.CompletedTask;
	}

	public Task UpdateAsync(User user, CancellationToken ct)
	{
		var idx = _users.FindIndex(u => u.Id == user.Id);
		if (idx >= 0) _users[idx] = user;
		return Task.CompletedTask;
	}
}
