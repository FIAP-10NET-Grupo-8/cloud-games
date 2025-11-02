using Fiap.CloudGames.Domain.Promotions.Enums;

namespace Fiap.CloudGames.Application.Promotions.Dtos;

/// <summary>
/// DTO usado para atualizar uma promoção existente.
/// </summary>
/// <param name="Name">Nome da promoção.</param>
/// <param name="StartDate">Data de início da promoção.</param>
/// <param name="EndDate">Data de término da promoção.</param>
/// <param name="Discount">Percentual de desconto (0-100).</param>
/// <param name="ElligibleGames">Coleção de identificadores dos jogos aplicáveis.</param>
/// <param name="Status">Status atual da promoção.</param>
public record UpdatePromotionDto(
	string Name,
	DateTime StartDate,
	DateTime EndDate,
	decimal Discount,
	IEnumerable<Guid> ElligibleGames,
	PromotionStatus? Status
);
