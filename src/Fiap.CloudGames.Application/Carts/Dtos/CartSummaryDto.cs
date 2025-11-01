namespace Fiap.CloudGames.Application.Carts.Dtos;

public sealed record CartSummaryDto(
    Guid CartId,
    string UserEmail,
    int ItemsCount,
    decimal Total,
    DateTime UpdatedAt
);
