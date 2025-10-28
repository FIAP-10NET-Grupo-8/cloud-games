using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Games.Repositories;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Infrastructure.Games.Repositories;

public sealed class GameRepository : IGameRepository
{
    private readonly AppDbContext _db;
    public GameRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<Game>> GetAllAsync(CancellationToken ct) =>
        await _db.Games.AsNoTracking().OrderBy(g => g.Title).ToListAsync(ct);

    public Task<Game?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Games.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task AddAsync(Game game, CancellationToken ct)
    {
        _db.Games.Add(game);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Game game, CancellationToken ct)
    {
        _db.Games.Update(game);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var e = await _db.Games.FindAsync(new object[] { id }, ct);
        if (e is null) return;
        _db.Games.Remove(e);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsByTitleAsync(string title, CancellationToken ct) =>
        _db.Games.AnyAsync(g => g.Title == title, ct);
}