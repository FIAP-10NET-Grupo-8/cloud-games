using Fiap.CloudGames.Application.DTOs;
using Fiap.CloudGames.Application.Interfaces;
using Fiap.CloudGames.Domain.Entities;
using Fiap.CloudGames.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                PlayerId = dto.PlayerId,
                TotalValue = dto.TotalValue,
                PaymentTransactionId = dto.PaymentTransactionId,
                PaymentConfirmed = true,
                PurchaseDate = DateTime.UtcNow,
                Status = "Pago"
            };

            await _repository.AddAsync(order);

            return new OrderResponseDto
            {
                Id = order.Id,
                PlayerId = order.PlayerId,
                PurchaseDate = order.PurchaseDate,
                TotalValue = order.TotalValue,
                Status = order.Status,
                RefundRequested = order.RefundRequested
            };
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? status)
        {
            var orders = await _repository.GetAllAsync(startDate, endDate, status);

            return orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                PlayerId = o.PlayerId,
                PurchaseDate = o.PurchaseDate,
                TotalValue = o.TotalValue,
                Status = o.Status,
                RefundRequested = o.RefundRequested
            });
        }

        public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order == null) return null;

            return new OrderResponseDto
            {
                Id = order.Id,
                PlayerId = order.PlayerId,
                PurchaseDate = order.PurchaseDate,
                TotalValue = order.TotalValue,
                Status = order.Status,
                RefundRequested = order.RefundRequested
            };
        }

        public async Task<bool> RequestRefundAsync(RefundRequestDto dto)
        {
            var order = await _repository.GetByIdAsync(dto.OrderId);
            if (order == null) return false;

            // Regras
            if (order.RefundRequested)
                throw new InvalidOperationException("Pedido já possui solicitação de estorno.");

            if ((DateTime.UtcNow - order.PurchaseDate).Days > 30)
                throw new InvalidOperationException("Pedido tem mais de 1 mês e não pode ser estornado.");

            order.RefundRequested = true;
            order.RefundRequestDate = DateTime.UtcNow;
            order.Status = "Estorno Solicitado";

            await _repository.UpdateAsync(order);
            return true;
        }
    }

}
