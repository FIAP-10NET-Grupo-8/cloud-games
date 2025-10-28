namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>
/// DTO para o Criação de Pedidos.   
/// </summary>
/// <param name="PlayerId"></param>
/// <param name="TotalValue"></param>
/// <param name="PaymentTransactionId"></param>
public record CreateOrderDto(Guid PlayerId, decimal TotalValue, string PaymentTransactionId);
