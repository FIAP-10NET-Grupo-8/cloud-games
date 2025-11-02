using Fiap.CloudGames.Application.Promotions.Dtos;

namespace Fiap.CloudGames.Application.Promotions.Services;

public interface IPromotionService
{
	Task<IReadOnlyList<PromotionDto>> GetAllAsync(CancellationToken ct);
	Task<PromotionDto?> GetByIdAsync(Guid id, CancellationToken ct);
	Task<PromotionDto> CreateAsync(CreatePromotionDto dto, CancellationToken ct);
	Task<bool> UpdateAsync(Guid id, UpdatePromotionDto dto, CancellationToken ct);
	Task<bool> DeactivateAsync(Guid id, CancellationToken ct);
}
