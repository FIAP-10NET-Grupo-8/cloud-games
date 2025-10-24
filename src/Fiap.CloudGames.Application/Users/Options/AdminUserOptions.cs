using Fiap.CloudGames.Domain.Users.Enums;

namespace Fiap.CloudGames.Application.Users.Options;

public class AdminUserOptions
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Status { get; set; } = default!;
    public bool EmailConfirmed { get; set; }

    public UserRole RoleAsEnum => Enum.TryParse<UserRole>(Role, ignoreCase: true, out var r) ? r : throw new ArgumentException("AdminUser: Role is invalid.", nameof(Role));
    public UserStatus StatusAsEnum => Enum.TryParse<UserStatus>(Status, ignoreCase: true, out var s) ? s : throw new ArgumentException("AdminUser: Status is invalid.", nameof(Status));

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("AdminUser: Name must be provided and non-empty.", nameof(Name));

        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("AdminUser: Email must be provided and non-empty.", nameof(Email));

        if (string.IsNullOrWhiteSpace(Password))
            throw new ArgumentException("AdminUser: Password must be provided and non-empty.", nameof(Password));

        if (string.IsNullOrWhiteSpace(Role))
            throw new ArgumentException("AdminUser: Role must be provided and non-empty.", nameof(Role));

        if (string.IsNullOrWhiteSpace(Status))
            throw new ArgumentException("AdminUser: Status must be provided and non-empty.", nameof(Status));

        // Validate email format using domain value object
        try
        {
            Fiap.CloudGames.Domain.Users.ValueObjects.Email.Create(Email);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"AdminUser: Email is invalid: {ex.Message}", nameof(Email));
        }

        // Validate password complexity using domain value object
        try
        {
            _ = Fiap.CloudGames.Domain.Users.ValueObjects.Password.Create(Password);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"AdminUser: Password is invalid: {ex.Message}", nameof(Password));
        }

        // Validate enums
        _ = RoleAsEnum;
        _ = StatusAsEnum;
    }
}
