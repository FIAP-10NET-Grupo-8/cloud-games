using Fiap.CloudGames.Domain.Shared.Options;
using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Domain.Users.Enums;
using Fiap.CloudGames.Domain.Users.Repositories;
using Microsoft.Extensions.Options;

namespace Fiap.CloudGames.Infrastructure.Users.Seeders;

public class UserSeeder(IUserRepository userRepository, IOptions<AdminUserOptions> options) : IUserSeeder
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly AdminUserOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

	public async Task SeedAsync(CancellationToken ct)
    {
        var adminEmail = _options.Email;
        var existing = await _userRepository.GetByEmailAsync(adminEmail, ct);
        if (existing is not null) return;

        var user = User.Create(
            name: _options.Name!,
            email: adminEmail!,
            password: _options.Password!,
            role: Enum.Parse<UserRole>(_options.Role!),
            status: Enum.Parse<UserStatus>(_options.Status!)
        );

        if (_options.EmailConfirmed)
            user.MarkEmailConfirmed();

        await _userRepository.AddAsync(user, ct);
    }
}
