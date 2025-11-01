using System.Security.Claims;
using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Carts.Dtos;

namespace Fiap.CloudGames.Application.Carts.Services;

/// <summary>
/// Contrato de aplicação para operações de carrinho.
/// </summary>
public interface ICartService
{
    /// <summary>Obtém o carrinho do usuário autenticado (cria se não existir).</summary>
    Task<CartResponseDto> GetMineAsync(ClaimsPrincipal user, CancellationToken ct);

    /// <summary>Adiciona um jogo ao carrinho (idempotente via header Idempotency-Key).</summary>
    Task<CartResponseDto> AddItemAsync(AddCartItemDto dto, string? idempotencyKey, ClaimsPrincipal user, CancellationToken ct);

    /// <summary>Remove um jogo do carrinho.</summary>
    Task<CartResponseDto> RemoveItemAsync(Guid gameId, ClaimsPrincipal user, CancellationToken ct);

    /// <summary>Limpa todos os itens do carrinho.</summary>
    Task<CartResponseDto> ClearAsync(ClaimsPrincipal user, CancellationToken ct);

    /// <summary>Lista carrinhos (admin), paginado.</summary>
    Task<PagedResult<CartSummaryDto>> GetAllAsync(int page, int pageSize, CancellationToken ct);
}
