using System.Security.Claims;
using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Orders.Dtos;

namespace Fiap.CloudGames.Application.Orders.Services;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(CreateOrderDto dto, string? idempotencyKey, ClaimsPrincipal user, CancellationToken ct);
    Task<PagedResult<OrderResponseDto>> GetForUserAsync(string userEmail, string? status, int page, int pageSize, CancellationToken ct);
    Task<PagedResult<OrderResponseDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? status, int page, int pageSize, CancellationToken ct);
    Task<OrderResponseDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> RequestRefundAsync(Guid id, RefundRequestDto dto, ClaimsPrincipal user, CancellationToken ct);
}