namespace Fiap.CloudGames.Application.Carts.Dtos;

public sealed record CartResponseDto(
    Guid CartId,
    Guid UserId,
    string UserEmail,
    decimal Total,
    DateTime UpdatedAt,
    IReadOnlyList<CartItemResponseDto> Items
);
