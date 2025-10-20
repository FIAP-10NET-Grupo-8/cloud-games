namespace Fiap.CloudGames.Infrastructure.Auth;

public class JwtOptions
{
	public required string Issuer { get; set; }
	public required string Audience { get; set; }
	public required string Secret { get; set; }
	public required int ExpiryMinutes { get; set; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(Issuer))
			throw new ArgumentException("Issuer must be provided and non-empty.", nameof(Issuer));

		if (string.IsNullOrWhiteSpace(Audience))
			throw new ArgumentException("Audience must be provided and non-empty.", nameof(Audience));

		if (string.IsNullOrWhiteSpace(Secret))
			throw new ArgumentException("Secret must be provided and non-empty.", nameof(Secret));

		if (Secret.Length < 32)
			throw new ArgumentException("Secret is too short. Use at least 32 characters in production.", nameof(Secret));

		if (ExpiryMinutes <= 0)
			throw new ArgumentException("ExpiryMinutes must be a positive integer.", nameof(ExpiryMinutes));
	}
}
