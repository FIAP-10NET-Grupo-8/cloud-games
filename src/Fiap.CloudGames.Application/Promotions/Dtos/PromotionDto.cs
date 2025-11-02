using Fiap.CloudGames.Application.Games.Dtos;
using Fiap.CloudGames.Domain.Promotions.Enums;

namespace Fiap.CloudGames.Application.Promotions.Dtos;

/// <summary>
/// DTO público com detalhes de uma promoção.
/// </summary>
/// <param name="Id">Identificador da promoção.</param>
/// <param name="Name">Nome da promoção.</param>
/// <param name="StartDate">Data de início da promoção.</param>
/// <param name="EndDate">Data de término da promoção.</param>
/// <param name="Discount">Percentual de desconto aplicado.</param>
/// <param name="ApplicableGames">Jogos aos quais a promoção se aplica.</param>
/// <param name="Status">Status atual da promoção.</param>
public record PromotionDto(
	Guid Id,
	string Name,
	DateTime StartDate,
	DateTime EndDate,
	decimal Discount,
	IEnumerable<GameListItemDto> ApplicableGames,
	PromotionStatus Status
);
