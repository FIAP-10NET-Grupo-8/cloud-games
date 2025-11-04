using Fiap.CloudGames.Application.Users.Dtos;

namespace Fiap.CloudGames.Application.Users.Services;

public interface IUserService
{
	Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct);
	Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct);
	Task<UserDto?> GetByEmailAsync(string email, CancellationToken ct);

	Task<LoginResultDto?> AuthenticateAsync(string email, string password, CancellationToken ct);

	Task<UserDto> RegisterAsync(UserRegisterDto dto, CancellationToken ct);
	Task<string> GenerateEmailConfirmationAsync(string email, CancellationToken ct);
	Task<bool> ConfirmEmailAsync(string token, CancellationToken ct);

	Task<string> GeneratePasswordResetAsync(string email, CancellationToken ct);
	Task<bool> ResetPasswordAsync(string token, string newPassword, CancellationToken ct);

	Task<string> GenerateFirstAccessAsync(string email, CancellationToken ct);
	Task<bool> FirstAccessAsync(string token, string newPassword, CancellationToken ct);

	Task<UserDto> CreateByAdminAsync(AdminUserCreateDto dto, CancellationToken ct);
	Task<UserDto?> UpdateAsync(AdminUserUpdateDto dto, CancellationToken ct);
	Task DeleteAsync(Guid id, CancellationToken ct);
	Task RestoreAsync(Guid id, CancellationToken ct);
}
