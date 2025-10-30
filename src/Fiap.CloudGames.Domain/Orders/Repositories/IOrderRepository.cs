using Fiap.CloudGames.Domain.Orders.Entities;

namespace Fiap.CloudGames.Domain.Orders.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<(IReadOnlyList<Order> Items, int Total)> QueryAllAsync(
        DateTime? startDate,
        DateTime? endDate,
        string? status,
        int page,
        int pageSize,
        CancellationToken ct);

    Task<(IReadOnlyList<Order> Items, int Total)> QueryForUserAsync(
        string userEmail,
        string? status,
        int page,
        int pageSize,
        CancellationToken ct);

    Task<Order?> GetByIdempotencyKeyAsync(Guid userId, string idempotencyKey, CancellationToken ct);

    Task AddAsync(Order order, CancellationToken ct);
    Task UpdateAsync(Order order, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}