using Fiap.CloudGames.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateAsync(CreateOrderDto dto);
        Task<IEnumerable<OrderResponseDto>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? status);
        Task<OrderResponseDto?> GetByIdAsync(Guid id);
        Task<bool> RequestRefundAsync(RefundRequestDto dto);
    }
}
