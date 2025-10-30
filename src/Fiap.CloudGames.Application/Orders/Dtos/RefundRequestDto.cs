namespace Fiap.CloudGames.Application.Orders.Dtos;

/// <summary>DTO para solicitar estorno.</summary>
/// <param name="Reason">Motivo do estorno.</param>
public record RefundRequestDto(string Reason);
