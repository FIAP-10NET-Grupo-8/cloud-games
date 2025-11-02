namespace Fiap.CloudGames.Domain.Promotions.ValueObjects;

public sealed record PromotionPeriod
{
	public DateTime StartDate { get; }
	public DateTime EndDate { get; }

	private PromotionPeriod(DateTime startDate, DateTime endDate)
	{
		StartDate = startDate;
		EndDate = endDate;
	}

	/// <summary>
	/// Factory Method to create a PromotionPeriod with validation.
	/// </summary>
	/// <param name="startDate"></param>
	/// <param name="endDate"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static PromotionPeriod Create(DateTime startDate, DateTime endDate)
	{
		if (endDate <= startDate)
			throw new ArgumentException("Data de término deve ser posterior à data de início.", nameof(endDate));
		return new(startDate, endDate);
	}

	/// <summary>
	/// Method to check if the promotion is active on a given date.
	/// </summary>
	/// <param name="currentDate"></param>
	/// <returns></returns>
	public bool IsActive(DateTime currentDate)
	{
		return currentDate >= StartDate && currentDate <= EndDate;
	}
}
