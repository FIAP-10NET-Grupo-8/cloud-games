using Fiap.CloudGames.Application.Carts.Dtos;
using Fiap.CloudGames.Application.Carts.Services;
using Fiap.CloudGames.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento do carrinho do usuário.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public sealed class CartsController(ICartService cartService) : ControllerBase
{
    private readonly ICartService _cartService = cartService;

    /// <summary>
    /// Obtém o carrinho do usuário autenticado (cria se não existir).
    /// </summary>
    [HttpGet("mine")]
    [Authorize]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(CancellationToken ct = default)
    {
        var cart = await _cartService.GetMineAsync(User, ct);
        return Ok(cart);
    }

    /// <summary>
    /// Adiciona um jogo ao carrinho do usuário autenticado.
    /// </summary>
    /// <param name="dto">Jogo a adicionar.</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpPost("mine/items")]
    [Authorize]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto dto, CancellationToken ct = default)
    {
        Request.Headers.TryGetValue("Idempotency-Key", out var idemKey);
        var updated = await _cartService.AddItemAsync(dto, idemKey.ToString(), User, ct);
        return Ok(updated);
    }

    /// <summary>
    /// Remove um jogo do carrinho do usuário autenticado.
    /// </summary>
    /// <param name="gameId">Identificador do jogo.</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpDelete("mine/items/{gameId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveItem(Guid gameId, CancellationToken ct = default)
    {
        var updated = await _cartService.RemoveItemAsync(gameId, User, ct);
        return Ok(updated);
    }

    /// <summary>
    /// Limpa todos os itens do carrinho do usuário autenticado.
    /// </summary>
    [HttpDelete("mine")]
    [Authorize]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Clear(CancellationToken ct = default)
    {
        var updated = await _cartService.ClearAsync(User, ct);
        return Ok(updated);
    }

    /// <summary>
    /// Lista todos os carrinhos (apenas Administrador) — útil para auditoria/testes.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    [ProducesResponseType(typeof(IEnumerable<CartSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var list = await _cartService.GetAllAsync(page, pageSize, ct);
        return Ok(list);
    }
}
