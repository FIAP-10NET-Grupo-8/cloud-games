namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO contendo token de confirmação de email.
/// </summary>
/// <param name="Token">Token de confirmação enviado por email.</param>
public record ConfirmEmailDto(string Token);
