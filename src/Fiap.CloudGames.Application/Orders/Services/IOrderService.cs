using Fiap.CloudGames.Application.Orders.Dtos;
using Fiap.CloudGames.Domain.Orders.Enums;

namespace Fiap.CloudGames.Application.Orders.Services;

public interface IOrderService
{
	Task<OrderResponseDto> CreateAsync(CreateOrderDto dto);
	Task<IEnumerable<OrderResponseDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, OrderStatus? status);
	Task<OrderResponseDto?> GetByIdAsync(Guid id);
	Task<bool> RequestRefundAsync(RefundRequestDto dto);
}
