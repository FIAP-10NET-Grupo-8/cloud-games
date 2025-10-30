namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>Item a ser incluído em um novo pedido.</summary>
/// <param name="GameId">Identificador do jogo.</param>
/// <param name="Quantity">Quantidade desejada.</param>
/// <param name="UnitPrice">Preço unitário no momento da compra (snapshot).</param>
public record CreateOrderItemDto(
    Guid GameId,
    int Quantity,
    decimal UnitPrice
);