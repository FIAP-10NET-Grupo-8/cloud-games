namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO utilizado para autenticação (login).
/// </summary>
/// <param name="Email">Endereço de email do usuário.</param>
/// <param name="Password">Senha em texto plano.</param>
public record LoginDto(string Email, string Password);
