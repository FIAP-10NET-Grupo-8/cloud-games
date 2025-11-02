using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Promotions.Enums;
using Fiap.CloudGames.Domain.Promotions.ValueObjects;
using System.Data;

namespace Fiap.CloudGames.Domain.Promotions.Entities;

/// <summary>
/// Entity representing a Promotion in the system.
/// </summary>
public class Promotion
{
	public Guid Id { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public PromotionPeriod Period { get; private set; } = default!;
	public Discount Discount { get; private set; } = default!;
	public IReadOnlyList<PromotionItem> EligibleGames => _eligibleGames.AsReadOnly();
	public PromotionStatus Status { get; private set; }

	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; private set; }

	private readonly List<PromotionItem> _eligibleGames = new();

	private Promotion() { }

	private Promotion(Guid id, string name, PromotionPeriod period, Discount discount, IEnumerable<PromotionItem> elligibleGames, PromotionStatus status, DateTime createdAt, DateTime? updatedAt)
	{
		Id = id;
		Name = name;
		Period = period;
		Discount = discount;
		_eligibleGames = [.. elligibleGames];
		Status = status;
		CreatedAt = createdAt;
		UpdatedAt = updatedAt;
	}

	/// <summary>
	/// Factory Method to create a new Promotion with validation and default values.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="startDate"></param>
	/// <param name="endDate"></param>
	/// <param name="discountPercentage"></param>
	/// <param name="applicableGames"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static Promotion Create(string name, DateTime startDate, DateTime endDate, decimal discountPercentage, IEnumerable<Guid>? elligibleGames = null)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Nome é obrigatório.", nameof(name));
		if (endDate <= startDate)
			throw new ArgumentException("Data de término deve ser posterior à data de início.", nameof(endDate));

		var period = PromotionPeriod.Create(startDate, endDate);
		var discount = Discount.Create(discountPercentage);

		var status = ComputeStatus(startDate, endDate);

		return new Promotion(Guid.NewGuid(), name.Trim(), period, discount, elligibleGames?.Select(id => new PromotionItem(id)) ?? [], status, DateTime.UtcNow, null);
	}

	/// <summary>
	/// Method to update the name of the promotion.
	/// </summary>
	/// <param name="name"></param>
	/// <exception cref="ArgumentException"></exception>
	public void UpdateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Nome é obrigatório.", nameof(name));
		Name = name.Trim();
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to update the start and end dates of the promotion.
	/// </summary>
	/// <param name="startDate"></param>
	/// <param name="endDate"></param>
	/// <exception cref="ArgumentException"></exception>
	public void UpdatePromotionDates(DateTime startDate, DateTime endDate)
	{
		var period = PromotionPeriod.Create(startDate, endDate);
		Period = period;
		UpdateStatus();
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to extend the end date of the promotion.
	/// </summary>
	/// <param name="newEndDate"></param>
	public void ExtendPromotion(DateTime newEndDate)
	{
		var period = PromotionPeriod.Create(Period.StartDate, newEndDate);
		Period = period;
		UpdateStatus();
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to update the discount percentage of the promotion.
	/// </summary>
	/// <param name="discountPercentage"></param>
	public void UpdateDiscount(decimal discountPercentage)
	{
		var discount = Discount.Create(discountPercentage);
		Discount = discount;
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to add a game to the applicable games of the promotion.
	/// </summary>
	/// <param name="gameId"></param>
	public void AddGame(PromotionItem gameId)
	{
		if (!EligibleGames.Contains(gameId))
		{
			_eligibleGames.Add(gameId);
			UpdatedAt = DateTime.UtcNow;
		}
	}

	/// <summary>
	/// Method to remove a game from the applicable games of the promotion.
	/// </summary>
	/// <param name="gameId"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public void RemoveGame(PromotionItem gameId)
	{
		if (EligibleGames.Count <= 1)
			throw new InvalidOperationException("Deve haver ao menos um jogo aplicável à promoção.");
		_eligibleGames.Remove(gameId);
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to update the applicable games of the promotion.
	/// </summary>
	/// <param name="elligibleGames"></param>
	/// <exception cref="ArgumentException"></exception>
	public void UpdateApplicableGames(IEnumerable<Guid>? elligibleGames)
	{
		if (elligibleGames == null || !elligibleGames.Any())
			throw new ArgumentException("Deve haver ao menos um jogo aplicável à promoção.", nameof(elligibleGames));
		_eligibleGames.Clear();
		_eligibleGames.AddRange(elligibleGames.Select(id => new PromotionItem(id)));
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to activate the promotion.
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	public void Activate()
	{
		if (!Period.IsActive(DateTime.UtcNow))
			throw new InvalidOperationException("A promoção só pode ser ativada dentro do período válido.");
		Status = PromotionStatus.Active;
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to deactivate the promotion.
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	public void Deactivate()
	{
		if (Status != PromotionStatus.Active && Status != PromotionStatus.Scheduled)
			throw new InvalidOperationException("A promoção só pode ser desativada se estiver ativa ou agendada.");
		Status = PromotionStatus.Inactive;
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to expire the promotion.
	/// </summary>
	/// <exception cref="InvalidOperationException"></exception>
	public void Expire()
	{
		if (DateTime.UtcNow < Period.EndDate)
			throw new InvalidOperationException("A promoção não pode ser expirada antes da data de término.");
		Status = PromotionStatus.Expired;
		UpdatedAt = DateTime.UtcNow;
	}

	/// <summary>
	/// Method to update the current status of the promotion based on start and end dates.
	/// </summary>
	public void UpdateStatus()
	{
		var utcNow = DateTime.UtcNow;
		if (utcNow < Period.StartDate) Status = PromotionStatus.Scheduled;
		else if (utcNow >= Period.StartDate && utcNow <= Period.EndDate) Status = PromotionStatus.Active;
		else Status = PromotionStatus.Expired;
		UpdatedAt = utcNow;
	}

	/// <summary>
	/// Method to compute the current status of the promotion based on start and end dates.
	/// </summary>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns></returns>
	public static PromotionStatus ComputeStatus(DateTime start, DateTime end)
	{
		var utcNow = DateTime.UtcNow;
		if (utcNow < start) return PromotionStatus.Scheduled;
		if (utcNow >= start && utcNow <= end) return PromotionStatus.Active;
		return PromotionStatus.Expired;
	}
}
