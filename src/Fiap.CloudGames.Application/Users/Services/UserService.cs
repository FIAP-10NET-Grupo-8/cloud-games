using Fiap.CloudGames.Application.Users.Dtos;
using Fiap.CloudGames.Domain.Shared.Interfaces;
using Fiap.CloudGames.Domain.Shared.Options;
using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Enums;
using Fiap.CloudGames.Domain.Users.Interfaces;
using Fiap.CloudGames.Domain.Users.Repositories;
using Microsoft.Extensions.Options;

namespace Fiap.CloudGames.Application.Users.Services;

public class UserService(IUserRepository repository, IJwtService jwtService, IEmailService emailService, IOptions<AdminUserOptions> adminOptions) : IUserService
{
	private readonly IUserRepository _repository = repository;
	private readonly IJwtService _jwtService = jwtService;
	private readonly IEmailService _emailService = emailService;
	private readonly string _superEmail = adminOptions.Value.Email.ToLowerInvariant();

	public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct)
	{
		var all = await _repository.GetAllAsync(ct);
		return all.Select(Map).ToList();
	}

	public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct)
	{
		var user = await _repository.GetByIdAsync(id, ct);
		return user == null ? null : Map(user);
	}

	public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct)
	{
		var user = await _repository.GetByEmailAsync(email, ct);
		return user == null ? null : Map(user);
	}

	public async Task<LoginResultDto?> AuthenticateAsync(string email, string password, CancellationToken ct)
	{
		var user = await _repository.GetByEmailAsync(email, ct);
		if (user == null) return null;
		if (!user.VerifyPassword(password)) return null;
		if (!user.EmailConfirmed) return null;
		if (user.Status != UserStatus.Active) return null;
		var (token, expiresAt) = _jwtService.GenerateToken(user.Id, user.Name, user.Email, user.Role.ToString(), ct);
		return new LoginResultDto(token, expiresAt);
	}

	public async Task<UserDto> RegisterAsync(UserRegisterDto dto, CancellationToken ct)
	{
		var existing = await _repository.GetByEmailAsync(dto.Email, ct);
		if (existing != null) throw new ArgumentException("Usuário com este e-mail já existe.");

		var user = User.Create(dto.Name, dto.Email, dto.Password, UserRole.User, UserStatus.Inactive);
		await _repository.AddAsync(user, ct);

		var token = await GenerateEmailConfirmationAsync(user.Email, ct);
		await _emailService.SendEmailAsync(user.Email, "Confirmação de email", $"Seu token de confirmação: {token}");

		return Map(user);
	}

	public async Task<string> GenerateEmailConfirmationAsync(string email, CancellationToken ct)
	{
		var user = await _repository.GetByEmailAsync(email, ct) ?? throw new ArgumentException("Usuário não encontrado.");
		var token = user.GenerateEmailConfirmationToken();
		await _repository.UpdateAsync(user, ct);
		return token;
	}

	public async Task<bool> ConfirmEmailAsync(string token, CancellationToken ct)
	{
		var user = await _repository.GetByConfirmationTokenAsync(token, ct);
		if (user == null) return false;
		var result = user.ConfirmEmail(token);
		if (result) await _repository.UpdateAsync(user, ct);
		return result;
	}

	public async Task<string> GeneratePasswordResetAsync(string email, CancellationToken ct)
	{
		var user = await _repository.GetByEmailAsync(email, ct) ?? throw new ArgumentException("Usuário não encontrado.");
		var token = user.GeneratePasswordResetToken(TimeSpan.FromHours(1));
		await _repository.UpdateAsync(user, ct);
		return token;
	}

	public async Task<bool> ResetPasswordAsync(string token, string newPassword, CancellationToken ct)
	{
		var user = await _repository.GetByPasswordResetTokenAsync(token, ct);
		if (user == null) return false;
		var result = user.ResetPassword(token, newPassword);
		if (result) await _repository.UpdateAsync(user, ct);
		return result;
	}

	public async Task<string> GenerateFirstAccessAsync(string email, CancellationToken ct)
	{
		var user = await _repository.GetByEmailAsync(email, ct) ?? throw new ArgumentException("Usuário não encontrado.");
		var token = user.GenerateFirstAccessToken(TimeSpan.FromHours(24));
		await _repository.UpdateAsync(user, ct);
		return token;
	}

	public async Task<bool> FirstAccessAsync(string token, string newPassword, CancellationToken ct)
	{
		var user = await _repository.GetByFirstAccessTokenAsync(token, ct);
		if (user == null) return false;
		var result = user.CompleteFirstAccess(token, newPassword);
		if (result) await _repository.UpdateAsync(user, ct);
		return result;
	}

	public async Task<UserDto> CreateByAdminAsync(AdminUserCreateDto dto, CancellationToken ct)
	{
		var existing = await _repository.GetByEmailAsync(dto.Email, ct);
		if (existing != null) throw new ArgumentException("Usuário com este e-mail já existe.");

		// The user will set their own password via first access flow, but we need to have a valid password at creation time.
		var tempPassword = GenerateTemporaryPassword();
		var user = User.Create(dto.Name, dto.Email, tempPassword, dto.Role, UserStatus.Inactive);
		await _repository.AddAsync(user, ct);

		var token = await GenerateFirstAccessAsync(user.Email, ct);
		await _emailService.SendEmailAsync(user.Email, "Acesso inicial - defina sua senha", $"Utilize este token para definir sua senha inicial: {token}");

		return Map(user);
	}

	public async Task<UserDto?> UpdateAsync(AdminUserUpdateDto dto, CancellationToken ct)
	{
		var user = await _repository.GetByIdAsync(dto.Id, ct);
		if (user == null) return null;

		// Prevent modifying the seeded super admin
		if (!string.IsNullOrWhiteSpace(_superEmail) && string.Equals(user.Email.Address, _superEmail, StringComparison.OrdinalIgnoreCase))
			throw new InvalidOperationException("O usuário administrativo principal não pode ser modificado.");

		if (!string.IsNullOrWhiteSpace(dto.Name)) user.UpdateName(dto.Name);

		if (!string.IsNullOrWhiteSpace(dto.Email))
		{
			if (!string.Equals(user.Email.Address, dto.Email, StringComparison.OrdinalIgnoreCase))
			{
				var other = await _repository.GetByEmailAsync(dto.Email, ct);
				if (other != null && other.Id != user.Id) throw new ArgumentException("Usuário com este e-mail já existe.");
			}
			user.UpdateEmail(dto.Email);
		}

		if (dto.Role.HasValue) user.SetRole(dto.Role.Value);
		if (dto.Status.HasValue) user.SetStatus(dto.Status.Value);
		if (dto.EmailConfirmed.HasValue)
		{
			if (dto.EmailConfirmed.Value) user.MarkEmailConfirmed(); else user.MarkEmailUnconfirmed();
		}

		await _repository.UpdateAsync(user, ct);
		return Map(user);
	}

	public async Task DeleteAsync(Guid id, CancellationToken ct)
	{
		var user = await _repository.GetByIdAsync(id, ct);
		if (user == null) return;

		// Prevent deleting the seeded super admin
		if (!string.IsNullOrWhiteSpace(_superEmail) && string.Equals(user.Email.Address, _superEmail, StringComparison.OrdinalIgnoreCase))
			throw new InvalidOperationException("O usuário administrativo principal não pode ser removido.");

		user.SoftDelete();
		await _repository.UpdateAsync(user, ct);
	}

	public async Task RestoreAsync(Guid id, CancellationToken ct)
	{
		var user = await _repository.GetByIdAsync(id, ct);
		if (user == null) return;
		user.Restore();
		await _repository.UpdateAsync(user, ct);
	}

	private static UserDto Map(User user) => new(user.Id, user.Name, user.Email.Address, user.Role, user.EmailConfirmed, user.CreatedAt);

	private static string GenerateTemporaryPassword()
	{
		var guidPart = Guid.NewGuid().ToString("N");
		return $"Aa1!{guidPart[..8]}";
	}
}
