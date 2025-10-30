using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Orders.Dtos;
using Fiap.CloudGames.Application.Orders.Services;
using Fiap.CloudGames.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de pedidos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public sealed class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    /// <summary>
    /// Cria um novo pedido a partir do carrinho/itens informados.
    /// </summary>
    /// <param name="dto">Dados do pedido (itens, valores, método de pagamento).</param>
    /// <param name="ct">Token de cancelamento.</param>
    /// <returns>Pedido criado.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
    {
        Request.Headers.TryGetValue("Idempotency-Key", out var idemKey);

        var created = await _orderService.CreateAsync(dto, idemKey.ToString(), User, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Lista pedidos do usuário autenticado (dono).
    /// </summary>
    /// <param name="status">Status opcional (ex.: PendingPayment/Paid/Cancelled/Refunded).</param>
    /// <param name="page">Página (>= 1).</param>
    /// <param name="pageSize">Itens por página (1–100).</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpGet("mine")]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var result = await _orderService.GetForUserAsync(userEmail!, status, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>
    /// Lista todos os pedidos (apenas Administrador).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = nameof(UserRole.Administrator))]
    [ProducesResponseType(typeof(PagedResult<OrderResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var result = await _orderService.GetAllAsync(startDate, endDate, status, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>
    /// Obtém um pedido por identificador (Admin ou dono).
    /// </summary>
    /// <param name="id">Identificador do pedido.</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpGet("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var order = await _orderService.GetByIdAsync(id, ct);
        if (order is null)
            return NotFound(Problem(title: "Pedido não encontrado.", statusCode: StatusCodes.Status404NotFound));

        var isAdmin = User.IsInRole(nameof(UserRole.Administrator));
        var isOwner = string.Equals(order.CustomerEmail, User.FindFirstValue(ClaimTypes.Email), StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner)
            return Forbid();

        return Ok(order);
    }

    /// <summary>
    /// Solicita estorno do pedido (Admin ou dono).
    /// </summary>
    /// <param name="id">Id do pedido.</param>
    /// <param name="dto">Motivo/observações do estorno (opcional).</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpPost("{id:guid}/refund")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RequestRefund(Guid id, [FromBody] RefundRequestDto dto, CancellationToken ct)
    {
        var order = await _orderService.GetByIdAsync(id, ct);
        if (order is null)
            return NotFound(Problem(title: "Pedido não encontrado.", statusCode: StatusCodes.Status404NotFound));

        var isAdmin = User.IsInRole(nameof(UserRole.Administrator));
        var isOwner = string.Equals(order.CustomerEmail, User.FindFirstValue(ClaimTypes.Email), StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner)
            return Forbid();

        var accepted = await _orderService.RequestRefundAsync(id, dto, User, ct);
        if (!accepted)
            return BadRequest(Problem(title: "Não foi possível solicitar o estorno.", statusCode: StatusCodes.Status400BadRequest));

        return Accepted(new { message = "Solicitação de estorno registrada." });
    }
}
