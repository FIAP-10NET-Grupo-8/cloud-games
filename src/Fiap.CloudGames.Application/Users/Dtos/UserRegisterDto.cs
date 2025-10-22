namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO usado para cadastro de usuário pelo próprio usuário (self-signup).
/// </summary>
/// <param name="Name">Nome completo do usuário.</param>
/// <param name="Email">Endereço de email.</param>
/// <param name="Password">Senha em texto plano (será validada/hasheada pelo domínio).</param>
public record UserRegisterDto(string Name, string Email, string Password);
