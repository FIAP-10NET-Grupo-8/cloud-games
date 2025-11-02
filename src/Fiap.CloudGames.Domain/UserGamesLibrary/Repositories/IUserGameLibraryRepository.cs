using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.ValueObjects;

namespace Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;

public interface IUserGameLibraryRepository
{
    /// <summary>Verifica se o usuário já possui o jogo.</summary>
    Task<bool> ExistsAsync(Guid userId, Guid gameId, CancellationToken ct);

    /// <summary>Obtém o vínculo (se existir) entre usuário e jogo.</summary>
    Task<UserGameLibrary?> GetAsync(Guid userId, Guid gameId, CancellationToken ct);

    /// <summary>Adiciona um item à biblioteca do usuário.</summary>
    Task AddAsync(UserGameLibrary userGameLibrary, CancellationToken ct);

    /// <summary>Remove um item da biblioteca do usuário.</summary>
    Task DeleteAsync(UserGameLibrary userGameLibrary, CancellationToken ct);

    /// <summary>
    /// Lista itens de biblioteca de um usuário com filtros e paginação.
    /// Retorna a coleção de vínculos (normalmente com Include(Game) na Infra) e o total.
    /// </summary>
    Task<(IReadOnlyList<UserGameLibrary> Items, int Total)> GetByUserIdAsync(
        Guid userId,
        LibraryFilter filter,
        CancellationToken ct);
}
