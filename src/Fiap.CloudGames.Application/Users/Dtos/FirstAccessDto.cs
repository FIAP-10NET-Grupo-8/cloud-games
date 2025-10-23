namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO usado no fluxo de primeiro acesso para definir a senha inicial.
/// </summary>
/// <param name="Token">Token recebido por email.</param>
/// <param name="NewPassword">Senha escolhida pelo usuário.</param>
public record FirstAccessDto(string Token, string NewPassword);
