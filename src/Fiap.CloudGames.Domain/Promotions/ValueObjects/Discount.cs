using System;

namespace Fiap.CloudGames.Domain.Promotions.ValueObjects;

/// <summary>
/// Value object representing a discount percentage (greater than 0 and up to 100).
/// </summary>
public sealed record Discount
{
	public decimal Percentage { get; }

	private Discount(decimal percentage)
	{
		Percentage = percentage;
	}

	/// <summary>
	/// Factory Method to create a Discount with validation.
	/// </summary>
	/// <param name="percentage"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public static Discount Create(decimal percentage)
	{
		if (percentage <= 0 || percentage > 100)
			throw new ArgumentOutOfRangeException(nameof(percentage), "Percentual de desconto deve ser > 0 e <= 100.");
		return new(Math.Round(percentage, 2));
	}

	public static implicit operator decimal(Discount discount) => discount.Percentage;
}
