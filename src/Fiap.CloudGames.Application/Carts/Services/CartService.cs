using System.Security.Claims;
using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Carts.Dtos;
using Fiap.CloudGames.Domain.Carts.Entities;
using Fiap.CloudGames.Domain.Carts.Repositories;
using Fiap.CloudGames.Domain.Games.Repositories;

namespace Fiap.CloudGames.Application.Carts.Services;

/// <summary>
/// Implementação de aplicação do carrinho, seguindo o padrão do OrderService.
/// </summary>
public sealed class CartService(ICartRepository repository, IGameRepository gameRepository) : ICartService
{
    private readonly ICartRepository _repository = repository;
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<CartResponseDto> GetMineAsync(ClaimsPrincipal user, CancellationToken ct)
    {
        var (userId, email) = GetAuth(user);

        var cart = await _repository.GetByUserIdAsync(userId, ct);
        if (cart is null)
        {
            cart = Cart.Create(userId, email);
            await _repository.AddAsync(cart, ct);
        }

        return ToDto(cart);
    }

    public async Task<CartResponseDto> AddItemAsync(AddCartItemDto dto, string? idempotencyKey, ClaimsPrincipal user, CancellationToken ct)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));
        if (dto.GameId == Guid.Empty) throw new ArgumentException("GameId inválido.", nameof(dto));

        var (userId, email) = GetAuth(user);

        var cart = await _repository.GetByUserIdAsync(userId, ct);
        if (cart is null)
        {
            cart = Cart.Create(userId, email);
            await _repository.AddAsync(cart, ct);
        }

        if (cart.Contains(dto.GameId))
            return ToDto(cart);

        var game = await _gameRepository.GetByIdAsync(dto.GameId, ct)
                   ?? throw new ArgumentException($"Jogo não encontrado: {dto.GameId}", nameof(dto.GameId));

        cart.AddItem(game.Id, game.Title, game.Price);

        await _repository.UpdateAsync(cart, ct);
        return ToDto(cart);
    }

    public async Task<CartResponseDto> RemoveItemAsync(Guid gameId, ClaimsPrincipal user, CancellationToken ct)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("GameId inválido.", nameof(gameId));

        var (userId, email) = GetAuth(user);
        var cart = await _repository.GetByUserIdAsync(userId, ct);

        if (cart is null)
        {
            cart = Cart.Create(userId, email);
            await _repository.AddAsync(cart, ct);
            return ToDto(cart);
        }

        cart.RemoveItem(gameId);
        await _repository.UpdateAsync(cart, ct);
        return ToDto(cart);
    }

    public async Task<CartResponseDto> ClearAsync(ClaimsPrincipal user, CancellationToken ct)
    {
        var (userId, email) = GetAuth(user);
        var cart = await _repository.GetByUserIdAsync(userId, ct);

        if (cart is null)
        {
            cart = Cart.Create(userId, email);
            await _repository.AddAsync(cart, ct);
            return ToDto(cart);
        }

        cart.Clear();
        await _repository.UpdateAsync(cart, ct);
        return ToDto(cart);
    }

    public async Task<PagedResult<CartSummaryDto>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        var (carts, total) = await _repository.QueryAllAsync(page, pageSize, ct);

        var summaries = carts.Select(c => new CartSummaryDto(
            c.Id,
            c.UserEmail,
            c.Items.Count,
            c.TotalValue,
            c.UpdatedAt ?? c.CreatedAt
        )).ToList();

        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        return new PagedResult<CartSummaryDto>(summaries, page, pageSize, total, totalPages);
    }

    private static (Guid userId, string email) GetAuth(ClaimsPrincipal user)
    {
        var tokenUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(tokenUserId, out var authUserId))
            throw new UnauthorizedAccessException("Token sem NameIdentifier válido.");

        var email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        return (authUserId, email);
    }

    private static CartResponseDto ToDto(Cart c) =>
        new(
            c.Id,
            c.UserId,
            c.UserEmail,
            c.TotalValue,
            c.UpdatedAt ?? c.CreatedAt,
            c.Items.Select(i => new CartItemResponseDto(
                i.GameId,
                i.Title,
                i.UnitPrice,
                i.Discount,
                i.FinalPrice
            )).ToList()
        );
}
