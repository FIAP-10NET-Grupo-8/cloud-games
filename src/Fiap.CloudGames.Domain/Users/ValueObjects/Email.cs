using System.Net.Mail;

namespace Fiap.CloudGames.Domain.Users.ValueObjects;

/// <summary>
/// Value Object representing an Email address with validation.
/// </summary>
public class Email
{
	public string Address { get; }

	private Email(string address)
	{
		Address = address;
	}

	/// <summary>
	/// Factory method to create and validate an Email value object.
	/// </summary>
	/// <param name="emailAddress"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static Email Create(string emailAddress)
	{
		if (string.IsNullOrWhiteSpace(emailAddress))
		{
			throw new ArgumentException("Endereço de email não pode ser vazio.", nameof(emailAddress));
		}

		try
		{
			var mailAddress = new MailAddress(emailAddress);
			if (mailAddress.Address != emailAddress)
			{
				throw new FormatException();
			}
		}
		catch (FormatException)
		{
			throw new ArgumentException("Formato de email inválido.", nameof(emailAddress));
		}

		return new Email(emailAddress);
	}

	public static implicit operator string(Email email) => email.Address;
}
