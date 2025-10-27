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

	public async Task<IReadOnlyList<User>> GetAllAsync()
	{
		return await _context.Users.ToListAsync();
	}

	public async Task<User?> GetByIdAsync(Guid id)
	{
		return await _context.Users.FindAsync(id);
	}

	public async Task<User?> GetByEmailAsync(string email)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.Email.Address.ToLower() == email.ToLower());
	}

	public async Task<User?> GetByConfirmationTokenAsync(string token)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.ConfirmationToken == token);
	}

	public async Task<User?> GetByPasswordResetTokenAsync(string token)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.PasswordResetToken == token);
	}

	public async Task<User?> GetByFirstAccessTokenAsync(string token)
	{
		return await _context.Users
			.FirstOrDefaultAsync(u => u.FirstAccessToken == token);
	}

	public async Task AddAsync(User user)
	{
		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(User user)
	{
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
	}
}
