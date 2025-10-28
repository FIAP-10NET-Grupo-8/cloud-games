using Fiap.CloudGames.Application.DTOs;
using Fiap.CloudGames.Application.Interfaces;
using Fiap.CloudGames.Domain.Entities;
using Fiap.CloudGames.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Fiap.CloudGames.Application.DTOs.GameDtos;

namespace Fiap.CloudGames.Api.Controllers
{
    //[Authorize] // Exige autenticação em todos os endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Endpoint paracriar o Pedido
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>
        /// Endpoint consulta todos os Pedidos com filtros opcionais
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? status)
        {
            var orders = await _orderService.GetAllAsync(startDate, endDate, status);
            return Ok(orders);
        }

        /// <summary>
        /// Endpoint consulta Pedido por Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        /// <summary>
        /// Endpoint para solicitar estorno de um Pedido
        /// </summary>
        [HttpPost("refund")]
        public async Task<IActionResult> RequestRefund([FromBody] RefundRequestDto dto)
        {
            var success = await _orderService.RequestRefundAsync(dto);
            if (!success) return BadRequest("Não foi possível solicitar o estorno.");
            return Ok("Solicitação de estorno registrada com sucesso.");
        }
    }

}
