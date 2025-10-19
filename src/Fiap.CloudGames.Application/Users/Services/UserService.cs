using Fiap.CloudGames.Application.Users.Dtos;
using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Enums;
using Fiap.CloudGames.Domain.Users.Repositories;

namespace Fiap.CloudGames.Application.Users.Services;

public class UserService(IUserRepository repository) : IUserService
{
	private readonly IUserRepository _repository = repository;

	public async Task<IReadOnlyList<UserDto>> GetAllAsync()
	{
		var all = await _repository.GetAllAsync();
		return all.Select(Map).ToList();
	}

	public async Task<UserDto?> GetByIdAsync(Guid id)
	{
		var user = await _repository.GetByIdAsync(id);
		return user == null ? null : Map(user);
	}

	public async Task<UserDto?> GetByEmailAsync(string email)
	{
		var user = await _repository.GetByEmailAsync(email);
		return user == null ? null : Map(user);
	}

	public async Task<UserDto> RegisterAsync(UserRegisterDto dto)
	{
		var user = User.Create(dto.Name, dto.Email, dto.Password, UserRole.User, UserStatus.Inactive);
		await _repository.AddAsync(user);
		return Map(user);
	}

	public async Task<string> GenerateEmailConfirmationAsync(string email)
	{
		var user = await _repository.GetByEmailAsync(email) ?? throw new ArgumentException("Usuário não encontrado.");
		var token = user.GenerateEmailConfirmationToken();
		await _repository.UpdateAsync(user);
		return token;
	}

	public async Task<bool> ConfirmEmailAsync(string token)
	{
		var user = await _repository.GetByConfirmationTokenAsync(token);
		if (user == null) return false;
		var result = user.ConfirmEmail(token);
		if (result) await _repository.UpdateAsync(user);
		return result;
	}

	public async Task<string> GeneratePasswordResetAsync(string email)
	{
		var user = await _repository.GetByEmailAsync(email) ?? throw new ArgumentException("Usuário não encontrado.");
		var token = user.GeneratePasswordResetToken(TimeSpan.FromHours(1));
		await _repository.UpdateAsync(user);
		return token;
	}

	public async Task<bool> ResetPasswordAsync(string token, string newPassword)
	{
		var user = await _repository.GetByPasswordResetTokenAsync(token);
		if (user == null) return false;
		var result = user.ResetPassword(token, newPassword);
		if (result) await _repository.UpdateAsync(user);
		return result;
	}

	public async Task<UserDto> CreateByAdminAsync(AdminUserCreateDto dto)
	{
		var user = User.Create(dto.Name, dto.Email, dto.Password, dto.Role, UserStatus.Inactive);
		await _repository.AddAsync(user);
		return Map(user);
	}

	public async Task<UserDto?> UpdateAsync(AdminUserUpdateDto dto)
	{
		var user = await _repository.GetByIdAsync(dto.Id);
		if (user == null) return null;

		if (!string.IsNullOrWhiteSpace(dto.Name)) user.UpdateName(dto.Name);
		if (!string.IsNullOrWhiteSpace(dto.Email)) user.UpdateEmail(dto.Email);
		if (dto.Role.HasValue) user.SetRole(dto.Role.Value);
		if (dto.Status.HasValue) user.SetStatus(dto.Status.Value);
		if (dto.EmailConfirmed.HasValue)
		{
			if (dto.EmailConfirmed.Value) user.MarkEmailConfirmed(); else user.MarkEmailUnconfirmed();
		}

		await _repository.UpdateAsync(user);
		return Map(user);
	}

	public async Task DeleteAsync(Guid id)
	{
		var user = await _repository.GetByIdAsync(id);
		if (user == null) return;
		user.SoftDelete();
		await _repository.UpdateAsync(user);
	}

	public async Task RestoreAsync(Guid id)
	{
		var user = await _repository.GetByIdAsync(id);
		if (user == null) return;
		user.Restore();
		await _repository.UpdateAsync(user);
	}

	private static UserDto Map(User user) => new(user.Id, user.Name, user.Email.Address, user.Role, user.EmailConfirmed, user.CreatedAt);
}
