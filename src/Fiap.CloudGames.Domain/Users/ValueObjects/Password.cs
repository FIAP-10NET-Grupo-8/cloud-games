namespace Fiap.CloudGames.Domain.Users.ValueObjects;

/// <summary>
/// Value Object representing a Password with hashing and validation.
/// </summary>
public class Password
{
	public string Hash { get; }

	private Password(string hash)
	{
		Hash = hash;
	}

	/// <summary>
	/// Factory method to create and validate a Password value object.
	/// </summary>
	/// <param name="plainTextPassword"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException">
	/// Throws if the password does not meet complexity requirements.
	/// </exception>
	public static Password Create(string plainTextPassword)
	{
		// Regra: Mínimo 8 caracteres com números, letras e caracteres especiais.

		if (string.IsNullOrWhiteSpace(plainTextPassword) || plainTextPassword.Length < 8)
		{
			throw new ArgumentException("A senha deve ter pelo menos 8 caracteres.");
		}

		bool hasLetter = false, hasDigit = false, hasSpecial = false;
		foreach (var c in plainTextPassword)
		{
			if (char.IsLetter(c)) hasLetter = true;
			else if (char.IsDigit(c)) hasDigit = true;
			else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
		}

		if (!(hasLetter && hasDigit && hasSpecial))
		{
			throw new ArgumentException("A senha deve conter letras, números e caracteres especiais.");
		}

		var passwordHash = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
		return new Password(passwordHash);
	}

	/// <summary>
	/// Method to verify a plain text password against the stored hashed password.
	/// </summary>
	/// <param name="plainTextPassword"></param>
	/// <returns></returns>
	public bool Verify(string plainTextPassword)
	{
		return BCrypt.Net.BCrypt.Verify(plainTextPassword, Hash);
	}
}
