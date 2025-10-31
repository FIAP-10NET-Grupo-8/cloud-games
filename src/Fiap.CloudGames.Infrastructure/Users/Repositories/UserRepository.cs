using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Repositories;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Infrastructure.Users.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
	private readonly AppDbContext _context = context;

	public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct)
	{
		return await _context.Users.ToListAsync(ct);
	}

	public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
	{
		return await _context.Users.FindAsync(new object?[] { id }, ct);
	}

	public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.Email.Address.ToLower() == email.ToLower(), ct);
	}

	public async Task<User?> GetByConfirmationTokenAsync(string token, CancellationToken ct)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.ConfirmationToken == token, ct);
	}

	public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.PasswordResetToken == token, ct);
	}

	public async Task<User?> GetByFirstAccessTokenAsync(string token, CancellationToken ct)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.FirstAccessToken == token, ct);
	}

	public async Task AddAsync(User user, CancellationToken ct)
	{
		await _context.Users.AddAsync(user, ct);
		await _context.SaveChangesAsync(ct);
	}

	public async Task UpdateAsync(User user, CancellationToken ct)
	{
		_context.Users.Update(user);
		await _context.SaveChangesAsync(ct);
	}
}
