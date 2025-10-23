namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO para solicitar redefinição de senha.
/// </summary>
/// <param name="Email">Email associado à conta.</param>
public record ForgotPasswordDto(string Email);
