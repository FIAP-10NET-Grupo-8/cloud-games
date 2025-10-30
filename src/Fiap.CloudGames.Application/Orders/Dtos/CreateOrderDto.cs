namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>DTO para criação de pedidos.</summary>
/// <param name="UserId">Identificador do usuário (dono do pedido).</param>
/// <param name="Items">Itens do pedido (com snapshot de preço).</param>
/// <param name="PaymentMethod">Método de pagamento (ex.: "Pix", "CreditCard").</param>
/// <param name="PaymentTransactionId">Identificador da transação no provedor (opcional).</param>
public record CreateOrderDto(
    Guid UserId,
    IReadOnlyList<CreateOrderItemDto> Items,
    string PaymentMethod,
    string? PaymentTransactionId
);
