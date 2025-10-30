using Fiap.CloudGames.Domain.Orders.Entities;
using Fiap.CloudGames.Domain.Orders.Enums;
using Fiap.CloudGames.Domain.Orders.Repositories;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fiap.CloudGames.Infrastructure.Orders.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    public OrderRepository(AppDbContext db) => _db = db;

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await _db.Orders.AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<(IReadOnlyList<Order> Items, int Total)> QueryAllAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? status,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var q = _db.Orders.AsNoTracking();

        if (startDate.HasValue) q = q.Where(o => o.CreatedAt >= startDate.Value);
        if (endDate.HasValue) q = q.Where(o => o.CreatedAt <= endDate.Value);

        if (TryParseStatus(status, out var st))
            q = q.Where(o => o.Status == st);

        var total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(o => o.Items)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<(IReadOnlyList<Order> Items, int Total)> QueryForUserAsync(
        string userEmail,
        string? status,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var q = _db.Orders.AsNoTracking()
            .Where(o => o.CustomerEmail == userEmail);

        if (TryParseStatus(status, out var st))
            q = q.Where(o => o.Status == st);

        var total = await q.CountAsync(ct);

        var items = await q
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(o => o.Items)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<Order?> GetByIdempotencyKeyAsync(Guid userId, string idempotencyKey, CancellationToken ct) =>
        await _db.Orders.AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.IdempotencyKey == idempotencyKey, ct);

    public async Task AddAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Order order, CancellationToken ct)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var e = await _db.Orders.FindAsync(new object[] { id }, ct);
        if (e is null) return;
        _db.Orders.Remove(e);
        await _db.SaveChangesAsync(ct);
    }

    private static bool TryParseStatus(string? input, out OrderStatus status)
    {
        if (!string.IsNullOrWhiteSpace(input) &&
            Enum.TryParse<OrderStatus>(input, true, out var parsed))
        {
            status = parsed;
            return true;
        }
        status = default;
        return false;
    }
}
