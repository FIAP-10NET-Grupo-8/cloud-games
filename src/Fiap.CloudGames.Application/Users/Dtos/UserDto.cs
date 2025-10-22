using Fiap.CloudGames.Domain.Users.Enums;

namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO público que representa um usuário para consumo por APIs.
/// </summary>
/// <param name="Id">Identificador único do usuário.</param>
/// <param name="Name">Nome do usuário.</param>
/// <param name="Email">Email do usuário.</param>
/// <param name="Role">Papel do usuário.</param>
/// <param name="EmailConfirmed">Se o email foi confirmado.</param>
/// <param name="CreatedAt">Data de criação da conta.</param>
public record UserDto(Guid Id, string Name, string Email, UserRole Role, bool EmailConfirmed, DateTime CreatedAt);
