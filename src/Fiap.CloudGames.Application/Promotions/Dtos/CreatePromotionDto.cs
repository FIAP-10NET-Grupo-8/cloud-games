namespace Fiap.CloudGames.Application.Promotions.Dtos;

/// <summary>
/// DTO usado para criar uma nova promoção.
/// </summary>
/// <param name="Name">Nome da promoção.</param>
/// <param name="StartDate">Data de início da promoção.</param>
/// <param name="EndDate">Data de término da promoção.</param>
/// <param name="Discount">Percentual de desconto (0-100).</param>
/// <param name="ElligibleGames">Coleção de identificadores dos jogos aplicáveis.</param>
public record CreatePromotionDto(
	string Name,
	DateTime StartDate,
	DateTime EndDate,
	decimal Discount,
	IEnumerable<Guid> ElligibleGames
);
