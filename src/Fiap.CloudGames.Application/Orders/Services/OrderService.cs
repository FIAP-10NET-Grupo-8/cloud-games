using Fiap.CloudGames.Application.Orders.Dtos;
using Fiap.CloudGames.Domain.Orders.Entities;
using Fiap.CloudGames.Domain.Orders.Enums;
using Fiap.CloudGames.Domain.Orders.Interfaces;

namespace Fiap.CloudGames.Application.Orders.Services;

public class OrderService(IOrderRepository repository) : IOrderService
{
	private readonly IOrderRepository _repository = repository;

	public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto)
	{
		var order = Order.Create(
			playerId: dto.PlayerId, 
			totalValue: dto.TotalValue, 
			paymentTransactionId: dto.PaymentTransactionId
		);
		await _repository.AddAsync(order);
		return Map(order);
	}

	public async Task<IEnumerable<OrderResponseDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, OrderStatus? status)
	{
		var orders = await _repository.GetAllAsync(startDate, endDate, status);
		return orders.Select(Map).ToList();
	}

	public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
	{
		var order = await _repository.GetByIdAsync(id);
		return order ==  null ? null : Map(order);
	}

	public async Task<bool> RequestRefundAsync(RefundRequestDto dto)
	{
		var order = await _repository.GetByIdAsync(dto.OrderId);
		if (order == null) return false;

		order.Refund(dto.Reason);
		await _repository.UpdateAsync(order);
		return true;
	}

	public static OrderResponseDto Map(Order order) => new(order.Id, order.PlayerId, order.PurchaseDate, order.TotalValue, order.Status.ToString(), order.RefundRequested);
}
