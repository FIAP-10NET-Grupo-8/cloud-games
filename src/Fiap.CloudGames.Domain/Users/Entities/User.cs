using Fiap.CloudGames.Domain.Users.Enums;
using Fiap.CloudGames.Domain.Users.UserObjects;

namespace Fiap.CloudGames.Domain.Users.Entities;

/// <summary>
/// Entity representing a User in the system.
/// </summary>
public class User
{
	public Guid Id { get; private set; }
	public string Name { get; private set; }
	public Email Email { get; private set; }
	public Password Password { get; private set; }
	public bool IsActive { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public UserRole Role { get; private set; }
	public UserStatus Status { get; private set; }

	private User(Guid id, string name, Email email, Password password, bool isActive, DateTime createdAt, UserRole role = UserRole.User, UserStatus status = UserStatus.Inactive)
	{
		Id = id;
		Name = name;
		Email = email;
		Password = password;
		IsActive = isActive;
		CreatedAt = createdAt;
		Role = role;
		Status = status;
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
			throw new ArgumentException("Name cannot be empty.", nameof(name));
		}

		var validatedEmail = Email.Create(email);
		var securePassword = Password.Create(password);

		return new User(Guid.NewGuid(), name, validatedEmail, securePassword, true, DateTime.UtcNow, role, status);
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
}
