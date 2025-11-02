namespace Fiap.CloudGames.Application.Carts.Dtos;

/// <summary>Item do carrinho.</summary>
/// <param name="GameId">Identificador do jogo.</param>
/// <param name="Title">Título (snapshot no momento da adição).</param>
/// <param name="UnitPrice">Preço unitário registrado.</param>
/// <param name="Discount">Desconto aplicado (se houver).</param>
/// <param name="FinalPrice">Preço final (UnitPrice - Discount).</param>
public sealed record CartItemResponseDto(
    Guid GameId,
    string Title,
    decimal UnitPrice,
    decimal Discount,
    decimal FinalPrice
);
