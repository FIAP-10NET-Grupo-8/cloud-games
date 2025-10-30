using System.Security.Claims;
using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Orders.Dtos;
using Fiap.CloudGames.Domain.Games.Repositories;
using Fiap.CloudGames.Domain.Orders.Entities;
using Fiap.CloudGames.Domain.Orders.Enums;
using Fiap.CloudGames.Domain.Orders.Repositories;

namespace Fiap.CloudGames.Application.Orders.Services;

public class OrderService(IOrderRepository repository, IGameRepository gameRepository) : IOrderService
{
    private readonly IOrderRepository _repository = repository;
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto, string? idempotencyKey, ClaimsPrincipal user, CancellationToken ct)
    {
        var tokenUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(tokenUserId, out var authUserId) || authUserId != dto.UserId)
            throw new UnauthorizedAccessException("Usuário não autorizado a criar pedido para outro usuário.");

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            var existing = await _repository.GetByIdempotencyKeyAsync(dto.UserId, idempotencyKey!, ct);
            if (existing is not null) return ToDto(existing);
        }

        var gameIds = dto.Items.Select(i => i.GameId).Distinct().ToList();
        var gamesById = new Dictionary<Guid, string>(gameIds.Count);
        foreach (var gid in gameIds)
        {
            var g = await _gameRepository.GetByIdAsync(gid, ct);
            if (g is null) throw new ArgumentException($"Jogo não encontrado: {gid}", nameof(dto.Items));
            gamesById[gid] = g.Title;
        }

        var orderItems = dto.Items
            .Select(i => OrderItem.Create(
                gameId: i.GameId,
                title: gamesById[i.GameId],
                quantity: i.Quantity,
                unitPrice: i.UnitPrice))
            .ToList();

        var customerEmail = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        var entity = Order.Create(
            userId: dto.UserId,
            customerEmail: customerEmail,
            items: orderItems,
            idempotencyKey: idempotencyKey,
            paymentTransactionId: dto.PaymentTransactionId
        );

        await _repository.AddAsync(entity, ct);
        return ToDto(entity);
    }

    public async Task<PagedResult<OrderResponseDto>> GetForUserAsync(string userEmail, string? status, int page, int pageSize, CancellationToken ct)
    {
        var (orders, total) = await _repository.QueryForUserAsync(userEmail, status, page, pageSize, ct);
        var dtos = orders.Select(ToDto).ToList();
        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        return new PagedResult<OrderResponseDto>(dtos, page, pageSize, total, totalPages);
    }

    public async Task<PagedResult<OrderResponseDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? status, int page, int pageSize, CancellationToken ct)
    {
        var (orders, total) = await _repository.QueryAllAsync(startDate, endDate, status, page, pageSize, ct);
        var dtos = orders.Select(ToDto).ToList();
        var totalPages = (int)Math.Ceiling((double)total / pageSize);
        return new PagedResult<OrderResponseDto>(dtos, page, pageSize, total, totalPages);
    }

    public async Task<OrderResponseDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        return order is null ? null : ToDto(order);
    }

    public async Task<bool> RequestRefundAsync(Guid id, RefundRequestDto dto, ClaimsPrincipal user, CancellationToken ct)
    {
        var order = await _repository.GetByIdAsync(id, ct);
        if (order is null) return false;

        var isAdmin = user.IsInRole("Administrator");
        var email = user.FindFirstValue(ClaimTypes.Email);
        var isOwner = string.Equals(order.CustomerEmail, email, StringComparison.OrdinalIgnoreCase);
        if (!isAdmin && !isOwner) throw new UnauthorizedAccessException("Sem permissão para solicitar estorno deste pedido.");

        if (order.Status is OrderStatus.Refunded or OrderStatus.Cancelled)
            throw new InvalidOperationException("Pedido não elegível para estorno.");
        if ((DateTime.UtcNow - order.CreatedAt).TotalDays > 30)
            throw new InvalidOperationException("Prazo para estorno expirou.");

        order.RequestRefund(dto.Reason, DateTime.UtcNow);

        await _repository.UpdateAsync(order, ct);
        return true;
    }

    private static OrderResponseDto ToDto(Order o) =>
        new(
            o.Id,
            o.UserId,
            o.CustomerEmail,
            o.CreatedAt,
            o.TotalValue,
            o.Status,
            o.RefundRequested,
            o.PaymentTransactionId,
            o.Items.Select(i =>
                new OrderItemResponseDto(i.GameId, i.Title, i.Quantity, i.UnitPrice, i.LineTotal)
            ).ToList()
        );
}