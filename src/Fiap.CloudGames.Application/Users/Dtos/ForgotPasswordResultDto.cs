namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO retornado após solicitação de recuperação de senha.
/// </summary>
/// <param name="ResetToken">Token para resetar a senha.</param>
public record ForgotPasswordResultDto(string ResetToken);
