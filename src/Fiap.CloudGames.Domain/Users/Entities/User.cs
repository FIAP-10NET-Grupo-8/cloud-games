using Fiap.CloudGames.Domain.Users.Enums;
using Fiap.CloudGames.Domain.Users.ValueObjects;

namespace Fiap.CloudGames.Domain.Users.Entities;

/// <summary>
/// Entity representing a User in the system.
/// </summary>
public class User
{
	public Guid Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public Email Email { get; private set; } = default!;
	public Password Password { get; private set; } = default!;
	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public UserRole Role { get; private set; } = UserRole.User;
	public UserStatus Status { get; private set; } = UserStatus.Inactive;
	public bool EmailConfirmed { get; private set; }
	public string? ConfirmationToken { get; private set; }
	public string? PasswordResetToken { get; private set; }
	public DateTime? PasswordResetExpiresAt { get; private set; }
	public string? FirstAccessToken { get; private set; }
	public DateTime? FirstAccessExpiresAt { get; private set; }

	public bool IsActive => Status == UserStatus.Active;

	private User() { }

	private User(Guid id, string name, Email email, Password password, DateTime createdAt, UserRole role = UserRole.User, UserStatus status = UserStatus.Inactive, bool emailConfirmed = false, string? confirmationToken = null, string? passwordResetToken = null, DateTime? passwordResetExpiresAt = null, string? firstAccessToken = null, DateTime? firstAccessExpiresAt = null)
	{
		Id = id;
		Name = name;
		Email = email;
		Password = password;
		CreatedAt = createdAt;
		Role = role;
		Status = status;
		EmailConfirmed = emailConfirmed;
		ConfirmationToken = confirmationToken;
		PasswordResetToken = passwordResetToken;
		PasswordResetExpiresAt = passwordResetExpiresAt;
		FirstAccessToken = firstAccessToken;
		FirstAccessExpiresAt = firstAccessExpiresAt;
	}

	/// <summary>
	/// Factory Method to create a new User with validation and default values.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <param name="role"></param>
	/// <param name="status"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static User Create(string name, string email, string password, UserRole role, UserStatus status)
	{

		if (string.IsNullOrEmpty(name?.Trim()))
		{
			throw new ArgumentException("O nome não pode ser vazio.", nameof(name));
		}

		var validatedEmail = Email.Create(email);
		var securePassword = Password.Create(password);

		return new User(Guid.NewGuid(), name, validatedEmail, securePassword, DateTime.UtcNow, role, status);
	}

	/// <summary>
	/// Method to update the user's name.
	/// </summary>
	/// <param name="name"></param>
	/// <exception cref="ArgumentException">
	/// Throws if the name is null or whitespace.
	/// </exception>
	public void UpdateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("O nome não pode ser vazio.", nameof(name));
		Name = name;
	}

	/// <summary>
	/// Method to update the user's email.
	/// </summary>
	/// <param name="email"></param>
	/// <remarks>
	/// Upon updating the email, the EmailConfirmed flag is reset to false.
	/// </remarks>
	public void UpdateEmail(string email)
	{
		var validated = Email.Create(email);
		Email = validated;
		MarkEmailUnconfirmed();
	}

	/// <summary>
	/// Method to set the user's role.
	/// </summary>
	/// <param name="role"></param>
	public void SetRole(UserRole role)
	{
		Role = role;
	}

	/// <summary>
	/// Method to set the user's status.
	/// </summary>
	/// <param name="status"></param>
	public void SetStatus(UserStatus status)
	{
		Status = status;
	}

	/// <summary>
	/// Method to soft delete the user by updating their status.
	/// </summary>
	public void SoftDelete()
	{
		Status = UserStatus.Deleted;
	}

	/// <summary>
	/// Method to restore a soft-deleted user.
	/// </summary>
	/// <remarks>
	/// Upon restoration, the user's status is set to Active if their email is confirmed; otherwise, it is set to Inactive.
	/// </remarks>
	public void Restore()
	{
		// If email already confirmed, restore to Active, otherwise to Inactive
		Status = EmailConfirmed ? UserStatus.Active : UserStatus.Inactive;
	}

	/// <summary>
	/// Method to verify a plain text password against the stored hashed password.
	/// </summary>
	/// <param name="plainTextPassword"></param>
	/// <returns></returns>
	public bool VerifyPassword(string plainTextPassword)
	{
		return Password.Verify(plainTextPassword);
	}

	/// <summary>
	/// Method to generate an email confirmation token.
	/// </summary>
	/// <returns></returns>
	public string GenerateEmailConfirmationToken()
	{
		ConfirmationToken = Guid.NewGuid().ToString("N");
		return ConfirmationToken;
	}

	/// <summary>
	/// Method to confirm email using the provided token.
	/// </summary>
	/// <param name="token"></param>
	/// <returns></returns>
	public bool ConfirmEmail(string token)
	{
		if (EmailConfirmed) return true;
		if (string.IsNullOrEmpty(ConfirmationToken)) return false;
		if (ConfirmationToken != token) return false;

		MarkEmailConfirmed();
		Status = UserStatus.Active;
		return true;
	}

	/// <summary>
	/// Method to mark the email as confirmed.
	/// </summary>
	public void MarkEmailConfirmed()
	{
		EmailConfirmed = true;
		ConfirmationToken = null;
	}

	/// <summary>
	/// Method to mark the email as unconfirmed.
	/// </summary>
	public void MarkEmailUnconfirmed()
	{
		EmailConfirmed = false;
	}

	/// <summary>
	/// Method to generate a password reset token valid for a specified duration.
	/// </summary>
	/// <param name="validFor"></param>
	/// <returns></returns>
	public string GeneratePasswordResetToken(TimeSpan validFor)
	{
		PasswordResetToken = Guid.NewGuid().ToString("N");
		PasswordResetExpiresAt = DateTime.UtcNow.Add(validFor);
		return PasswordResetToken;
	}

	/// <summary>
	/// Method to reset password using the provided token and new plain text password.
	/// </summary>
	/// <param name="token"></param>
	/// <param name="newPlainPassword"></param>
	/// <returns></returns>
	public bool ResetPassword(string token, string newPlainPassword)
	{
		if (string.IsNullOrEmpty(PasswordResetToken) || PasswordResetToken != token) return false;
		if (!PasswordResetExpiresAt.HasValue || PasswordResetExpiresAt.Value < DateTime.UtcNow) return false;

		var newPassword = Password.Create(newPlainPassword);
		Password = newPassword;
		PasswordResetToken = null;
		PasswordResetExpiresAt = null;
		return true;
	}

	/// <summary>
	/// Generates a token for first access (invitation) flow.
	/// </summary>
	/// <param name="validFor"></param>
	/// <returns></returns>
	public string GenerateFirstAccessToken(TimeSpan validFor)
	{
		FirstAccessToken = Guid.NewGuid().ToString("N");
		FirstAccessExpiresAt = DateTime.UtcNow.Add(validFor);
		return FirstAccessToken;
	}

	/// <summary>
	/// Completes first access using the token and sets the initial password.
	/// </summary>
	/// <param name="token"></param>
	/// <param name="newPlainPassword"></param>
	/// <returns></returns>
	public bool CompleteFirstAccess(string token, string newPlainPassword)
	{
		if (string.IsNullOrEmpty(FirstAccessToken) || FirstAccessToken != token) return false;
		if (!FirstAccessExpiresAt.HasValue || FirstAccessExpiresAt.Value < DateTime.UtcNow) return false;

		var newPassword = Password.Create(newPlainPassword);
		Password = newPassword;
		FirstAccessToken = null;
		FirstAccessExpiresAt = null;

		MarkEmailConfirmed();
		Status = UserStatus.Active;
		return true;
	}
}
