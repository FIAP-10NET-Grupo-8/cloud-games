using Fiap.CloudGames.Application.Users.Dtos;

namespace Fiap.CloudGames.Application.Users.Services;

public interface IUserService
{
	Task<IReadOnlyList<UserDto>> GetAllAsync();
	Task<UserDto?> GetByIdAsync(Guid id);
	Task<UserDto?> GetByEmailAsync(string email);

	Task<UserDto?> AuthenticateAsync(string email, string password);

	Task<UserDto> RegisterAsync(UserRegisterDto dto);
	Task<string> GenerateEmailConfirmationAsync(string email);
	Task<bool> ConfirmEmailAsync(string token);

	Task<string> GeneratePasswordResetAsync(string email);
	Task<bool> ResetPasswordAsync(string token, string newPassword);

	Task<string> GenerateFirstAccessAsync(string email);
	Task<bool> FirstAccessAsync(string token, string newPassword);

	Task<UserDto> CreateByAdminAsync(AdminUserCreateDto dto);
	Task<UserDto?> UpdateAsync(AdminUserUpdateDto dto);
	Task DeleteAsync(Guid id);
	Task RestoreAsync(Guid id);
}
