namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO retornado após autenticação bem-sucedida.
/// </summary>
/// <param name="Token">JWT token.</param>
/// <param name="ExpiresAt">Data e hora de expiração do token.</param>
public record LoginResultDto(string Token, DateTime ExpiresAt);
