using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;
using Fiap.CloudGames.Domain.UserGamesLibrary.ValueObjects;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Infrastructure.UserGamesLibrary.Repositories;

public class UserGameLibraryRepository : IUserGameLibraryRepository
{
    private readonly AppDbContext _db;
    public UserGameLibraryRepository(AppDbContext db) => _db = db;

    public async Task<bool> ExistsAsync(Guid userId, Guid gameId, CancellationToken ct)
        => await _db.UserGameLibrary
            .AsNoTracking()
            .AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId, ct);

    public async Task<UserGameLibrary?> GetAsync(Guid userId, Guid gameId, CancellationToken ct)
        => await _db.UserGameLibrary
            .AsNoTracking()
            .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId, ct);

    public async Task AddAsync(UserGameLibrary userGameLibrary, CancellationToken ct)
    {
        await _db.UserGameLibrary.AddAsync(userGameLibrary, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(UserGameLibrary userGameLibrary, CancellationToken ct)
    {
        _db.UserGameLibrary.Remove(userGameLibrary);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<(IReadOnlyList<UserGameLibrary> Items, int Total)> GetByUserIdAsync(
        Guid userId,
        LibraryFilter filter,
        CancellationToken ct)
    {
        IQueryable<UserGameLibrary> query = _db.UserGameLibrary
            .AsNoTracking()
            .Include(ug => ug.Game)
            .Where(ug => ug.UserId == userId);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.Trim();
            query = query.Where(ug =>
                ug.Game != null && (
                    EF.Functions.Like(ug.Game.Title, $"%{s}%") ||
                    EF.Functions.Like(ug.Game.Developer, $"%{s}%") ||
                    EF.Functions.Like(ug.Game.Publisher, $"%{s}%")
                ));
        }

        if (!string.IsNullOrWhiteSpace(filter.Genre))
        {
            var g = filter.Genre.Trim();
            query = query.Where(ug => ug.Game != null && ug.Game.Genre != null &&
                                      EF.Functions.Like(ug.Game.Genre, $"%{g}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Developer))
        {
            var d = filter.Developer.Trim();
            query = query.Where(ug => ug.Game != null &&
                                      EF.Functions.Like(ug.Game.Developer, $"%{d}%"));
        }

        if (!string.IsNullOrWhiteSpace(filter.Publisher))
        {
            var p = filter.Publisher.Trim();
            query = query.Where(ug => ug.Game != null &&
                                      EF.Functions.Like(ug.Game.Publisher, $"%{p}%"));
        }

        if (filter.StartDate.HasValue)
            query = query.Where(ug => ug.Game != null && ug.Game.ReleaseDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(ug => ug.Game != null && ug.Game.ReleaseDate <= filter.EndDate.Value);

        var total = await query.CountAsync(ct);

        var sortBy = filter.SortBy?.ToLowerInvariant();
        query = sortBy switch
        {
            "genre" => (filter.Desc
                ? query.OrderByDescending(ug => ug.Game!.Genre)
                : query.OrderBy(ug => ug.Game!.Genre)),
            "developer" => (filter.Desc
                ? query.OrderByDescending(ug => ug.Game!.Developer)
                : query.OrderBy(ug => ug.Game!.Developer)),
            "publisher" => (filter.Desc
                ? query.OrderByDescending(ug => ug.Game!.Publisher)
                : query.OrderBy(ug => ug.Game!.Publisher)),
            _ => (filter.Desc
                ? query.OrderByDescending(ug => ug.Game!.Title)
                : query.OrderBy(ug => ug.Game!.Title)),
        };

        var page = Math.Max(1, filter.Page);
        var pageSize = Math.Clamp(filter.PageSize, 1, 100);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }
}
