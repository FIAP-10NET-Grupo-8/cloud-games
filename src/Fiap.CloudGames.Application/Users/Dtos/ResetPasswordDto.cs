namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO para redefinir senha usando token.
/// </summary>
/// <param name="Token">Token de redefinição enviado por email.</param>
/// <param name="NewPassword">Nova senha em texto plano.</param>
public record ResetPasswordDto(string Token, string NewPassword);
