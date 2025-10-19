using Fiap.CloudGames.Domain.Users.Enums;

namespace Fiap.CloudGames.Application.Users.Dtos;

/// <summary>
/// DTO usado pelo administrador para atualizar um usuário.
/// Campos opcionais permitem atualizações parciais.
/// </summary>
/// <param name="Id">Identificador do usuário.</param>
/// <param name="Name">Novo nome (opcional).</param>
/// <param name="Email">Novo email (opcional).</param>
/// <param name="Role">Novo papel (opcional).</param>
/// <param name="Status">Novo status (opcional).</param>
/// <param name="EmailConfirmed">Marcar email como confirmado/desconfirmado (opcional).</param>
public record AdminUserUpdateDto(Guid Id, string? Name, string? Email, UserRole? Role, UserStatus? Status, bool? EmailConfirmed);
