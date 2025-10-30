using Fiap.CloudGames.Domain.Orders.Enums;

namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>DTO para consulta de pedidos.</summary>
/// <param name="Id">Identificador do pedido.</param>
/// <param name="UserId">Identificador do usuário (dono).</param>
/// <param name="CustomerEmail">Email do dono.</param>
/// <param name="CreatedAt">Data/hora de criação do pedido.</param>
/// <param name="TotalValue">Valor total do pedido.</param>
/// <param name="Status">Status atual do pedido.</param>
/// <param name="RefundRequested">Indica se há solicitação de estorno.</param>
/// <param name="PaymentTransactionId">Identificador da transação no provedor (quando houver).</param>
/// <param name="Items">Itens do pedido.</param>
public record OrderResponseDto(
    Guid Id,
    Guid UserId,
    string CustomerEmail,
    DateTime CreatedAt,
    decimal TotalValue,
    OrderStatus Status,
    bool RefundRequested,
    string? PaymentTransactionId,
    IReadOnlyList<OrderItemResponseDto> Items
);
