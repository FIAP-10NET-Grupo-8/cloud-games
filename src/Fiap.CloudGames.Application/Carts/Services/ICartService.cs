using System.Security.Claims;
using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Carts.Dtos;

namespace Fiap.CloudGames.Application.Carts.Services;

public interface ICartService
{
    Task<CartResponseDto> GetMineAsync(ClaimsPrincipal user, CancellationToken ct);
    Task<CartResponseDto> AddItemAsync(AddCartItemDto dto, string? idempotencyKey, ClaimsPrincipal user, CancellationToken ct);
    Task<CartResponseDto> RemoveItemAsync(Guid gameId, ClaimsPrincipal user, CancellationToken ct);
    Task<CartResponseDto> ClearAsync(ClaimsPrincipal user, CancellationToken ct);
    Task<PagedResult<CartSummaryDto>> GetAllAsync(int page, int pageSize, CancellationToken ct);
}
