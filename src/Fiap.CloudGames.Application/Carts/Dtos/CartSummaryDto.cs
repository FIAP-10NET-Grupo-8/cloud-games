namespace Fiap.CloudGames.Application.Carts.Dtos;

/// <summary>Resumo de carrinho (admin).</summary>
/// <param name="CartId">Identificador do carrinho.</param>
/// <param name="UserEmail">Email do dono.</param>
/// <param name="ItemsCount">Quantidade de itens.</param>
/// <param name="Total">Total do carrinho.</param>
/// <param name="UpdatedAt">Última atualização.</param>
public sealed record CartSummaryDto(
    Guid CartId,
    string UserEmail,
    int ItemsCount,
    decimal Total,
    DateTime UpdatedAt
);
