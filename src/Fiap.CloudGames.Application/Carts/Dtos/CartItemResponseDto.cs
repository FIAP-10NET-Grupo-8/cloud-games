namespace Fiap.CloudGames.Application.Carts.Dtos;

public sealed record CartItemResponseDto(
    Guid GameId,
    string Title,
    decimal UnitPrice,
    decimal Discount,
    decimal FinalPrice
);
