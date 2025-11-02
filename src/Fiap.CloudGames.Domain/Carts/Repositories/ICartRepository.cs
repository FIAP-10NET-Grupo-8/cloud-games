using Fiap.CloudGames.Domain.Carts.Entities;

namespace Fiap.CloudGames.Domain.Carts.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task AddAsync(Cart cart, CancellationToken ct);
    Task UpdateAsync(Cart cart, CancellationToken ct);
    Task<(IReadOnlyList<Cart> Carts, int Total)> QueryAllAsync(int page, int pageSize, CancellationToken ct);
}
