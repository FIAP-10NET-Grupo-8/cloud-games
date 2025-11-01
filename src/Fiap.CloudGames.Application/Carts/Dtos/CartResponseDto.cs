namespace Fiap.CloudGames.Application.Carts.Dtos;

/// <summary>Carrinho do usuário.</summary>
/// <param name="CartId">Identificador do carrinho.</param>
/// <param name="UserId">Identificador do usuário.</param>
/// <param name="UserEmail">Email do usuário.</param>
/// <param name="Total">Total do carrinho.</param>
/// <param name="UpdatedAt">Última atualização.</param>
/// <param name="Items">Itens do carrinho.</param>
public sealed record CartResponseDto(
    Guid CartId,
    Guid UserId,
    string UserEmail,
    decimal Total,
    DateTime UpdatedAt,
    IReadOnlyList<CartItemResponseDto> Items
);
