using Fiap.CloudGames.Domain.Carts.Entities;
using Fiap.CloudGames.Domain.Carts.Repositories;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Infrastructure.Carts.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly AppDbContext _db;
    public CartRepository(AppDbContext db) => _db = db;

    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct) =>
        await _db.Carts.AsNoTracking()
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task AddAsync(Cart cart, CancellationToken ct)
    {
        _db.Carts.Add(cart);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Cart cart, CancellationToken ct)
    {
        _db.Carts.Update(cart);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<(IReadOnlyList<Cart> Carts, int Total)> QueryAllAsync(int page, int pageSize, CancellationToken ct)
    {
        var q = _db.Carts.AsNoTracking();

        var total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.Items)
            .ToListAsync(ct);

        return (items, total);
    }
}
