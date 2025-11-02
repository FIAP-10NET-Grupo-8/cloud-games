using Fiap.CloudGames.Domain.Games.Entities;

namespace Fiap.CloudGames.Domain.Games.Repositories;

public interface IGameRepository
{
    Task<IReadOnlyList<Game>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<Game>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct);
    Task<Game?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Game game, CancellationToken ct);
    Task UpdateAsync(Game game, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByTitleAsync(string title, CancellationToken ct);
}