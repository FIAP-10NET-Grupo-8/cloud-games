namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>
/// DTO para consulta de Pedidos.
/// </summary>
/// <param name="Id"></param>
/// <param name="PlayerId"></param>
/// <param name="PurchaseDate"></param>
/// <param name="TotalValue"></param>
/// <param name="Status"></param>
/// <param name="RefundRequested"></param>
public record OrderResponseDto(Guid Id, Guid PlayerId, DateTime PurchaseDate, decimal TotalValue, string Status, bool RefundRequested);
