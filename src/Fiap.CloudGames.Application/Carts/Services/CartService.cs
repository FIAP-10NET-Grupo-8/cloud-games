using System.Security.Claims;
using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Carts.Dtos;
using Fiap.CloudGames.Domain.Carts.Repositories;
using Fiap.CloudGames.Domain.Games.Repositories;

namespace Fiap.CloudGames.Application.Carts.Services;

public sealed class CartService : ICartService
{
    private readonly ICartRepository _repository;
    private readonly IGameRepository _gameRepository;

    public CartService(ICartRepository repository, IGameRepository gameRepository)
    {
        _repository = repository;
        _gameRepository = gameRepository;
    }

    public Task<CartResponseDto> GetMineAsync(ClaimsPrincipal user, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CartResponseDto> AddItemAsync(AddCartItemDto dto, string? idempotencyKey, ClaimsPrincipal user, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CartResponseDto> RemoveItemAsync(Guid gameId, ClaimsPrincipal user, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<CartResponseDto> ClearAsync(ClaimsPrincipal user, CancellationToken ct)
        => throw new NotImplementedException();

    public Task<PagedResult<CartSummaryDto>> GetAllAsync(int page, int pageSize, CancellationToken ct)
        => throw new NotImplementedException();
}
