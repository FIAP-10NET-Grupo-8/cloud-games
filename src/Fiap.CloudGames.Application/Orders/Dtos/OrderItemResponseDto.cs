namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>Item retornado em um pedido.</summary>
/// <param name="GameId">Identificador do jogo.</param>
/// <param name="Title">Título do jogo no momento da compra (snapshot).</param>
/// <param name="Quantity">Quantidade comprada.</param>
/// <param name="UnitPrice">Preço unitário registrado.</param>
/// <param name="LineTotal">Total da linha (Quantity × UnitPrice).</param>
public record OrderItemResponseDto(
    Guid GameId,
    string Title,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal
);