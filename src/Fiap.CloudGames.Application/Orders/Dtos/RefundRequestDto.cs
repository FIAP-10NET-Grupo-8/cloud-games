namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>
/// Dto para o fluxo "Solicitar Estorno".
/// </summary>
/// <param name="OrderId"></param>
/// <param name="Reason"></param>
public record RefundRequestDto(Guid OrderId, string? Reason);
