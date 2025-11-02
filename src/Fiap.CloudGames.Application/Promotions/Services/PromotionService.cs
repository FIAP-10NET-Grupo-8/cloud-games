using Fiap.CloudGames.Application.Games.Dtos;
using Fiap.CloudGames.Application.Promotions.Dtos;
using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Games.Repositories;
using Fiap.CloudGames.Domain.Promotions.Entities;
using Fiap.CloudGames.Domain.Promotions.Enums;
using Fiap.CloudGames.Domain.Promotions.Repositories;

namespace Fiap.CloudGames.Application.Promotions.Services;

public class PromotionService(IPromotionRepository promotionRepository, IGameRepository gameRepository) : IPromotionService
{
	private readonly IPromotionRepository _promotionRepository = promotionRepository;
	private readonly IGameRepository _gameRepository = gameRepository;

	public async Task<IReadOnlyList<PromotionDto>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var promotions = await _promotionRepository.GetAllAsync(cancellationToken);
		if (promotions.Count == 0) return [];

		var allGameIds = promotions
			.SelectMany(p => p.EligibleGames.Select(eg => eg.GameId))
			.Distinct();

		var games = await _gameRepository.GetByIdsAsync(allGameIds, cancellationToken);

		return [.. promotions.Select(promotion => Map(promotion, games))];
	}

	public async Task<PromotionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var promotion = await _promotionRepository.GetByIdAsync(id, cancellationToken);
		if (promotion == null) return null;
		var gameIds = promotion.EligibleGames.Select(g => g.GameId);
		var games = await _gameRepository.GetByIdsAsync(gameIds, cancellationToken);
		return Map(promotion, games);
	}

	public async Task<PromotionDto> CreateAsync(CreatePromotionDto dto, CancellationToken cancellationToken = default)
	{
		var games = await _gameRepository.GetByIdsAsync(dto.ElligibleGames, cancellationToken);

		var promotion = Promotion.Create(dto.Name, dto.StartDate, dto.EndDate, dto.Discount, dto.ElligibleGames);
		await _promotionRepository.AddAsync(promotion, cancellationToken);

		return Map(promotion, games);
	}

	public async Task<bool> UpdateAsync(Guid id, UpdatePromotionDto dto, CancellationToken cancellationToken = default)
	{
		var promotion = await _promotionRepository.FindByIdAsync(id, cancellationToken);
		if (promotion == null) return false;

		if (promotion.Name != dto.Name) promotion.UpdateName(dto.Name);
		if (promotion.Period.StartDate != dto.StartDate || promotion.Period.EndDate != dto.EndDate) promotion.UpdatePromotionDates(dto.StartDate, dto.EndDate);
		if (promotion.Discount != dto.Discount) promotion.UpdateDiscount(dto.Discount);
		promotion.UpdateApplicableGames(dto.ElligibleGames);
		if (dto.Status.HasValue && promotion.Status != dto.Status)
		{
			switch(dto.Status.Value)
			{
				case PromotionStatus.Active:
					promotion.Activate();
					break;
				case PromotionStatus.Inactive:
					promotion.Deactivate();
					break;
				case PromotionStatus.Scheduled:
					promotion.UpdateStatus();
					break;
				case PromotionStatus.Expired:
					promotion.Expire();
					break;
			}
		}
		await _promotionRepository.UpdateAsync(promotion, cancellationToken);
		return true;
	}

	public async Task<bool> DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var p = await _promotionRepository.FindByIdAsync(id, cancellationToken);
		if (p == null) return false;
		p.Deactivate();
		await _promotionRepository.UpdateAsync(p, cancellationToken);
		return true;
	}

	private static PromotionDto Map(Promotion promotion, IEnumerable<Game> games)
	{
		var applicableGames = games
			.Where(g => promotion.EligibleGames.Any(pg => pg.GameId == g.Id))
			.Select(g => new GameListItemDto(g.Id, g.Title, g.Price, g.Active))
			.ToList();
		return new PromotionDto(promotion.Id, promotion.Name, promotion.Period.StartDate, promotion.Period.EndDate, promotion.Discount.Percentage, applicableGames, promotion.Status);
	}
}
