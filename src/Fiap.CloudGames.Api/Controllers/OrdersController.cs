using Fiap.CloudGames.Application.Orders.Dtos;
using Fiap.CloudGames.Application.Orders.Services;
using Fiap.CloudGames.Domain.Orders.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.CloudGames.Api.Controllers
{
	/// <summary>
	/// Endpoints para gerenciamento de Pedidos.
	/// </summary>
	/// <param name="orderService"></param>
	[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        /// <summary>
        /// Endpoint consulta todos os Pedidos com filtros opcionais
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] OrderStatus? status)
        {
            var orders = await _orderService.GetAllAsync(startDate, endDate, status);
            return Ok(orders);
        }

        /// <summary>
        /// Endpoint consulta Pedido por Id
        /// </summary>
        [HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
		{
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound(new { message = "Pedido não encontrado." });
            return Ok(order);
		}

		/// <summary>
		/// Endpoint para criar o Pedido
		/// </summary>
		[HttpPost]
		[ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
		{
			var order = await _orderService.CreateAsync(dto);
			return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
		}

		/// <summary>
		/// Endpoint para solicitar estorno de um Pedido
		/// </summary>
		[HttpPost("refund")]
		[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> RequestRefund([FromBody] RefundRequestDto dto)
        {
            var success = await _orderService.RequestRefundAsync(dto);
            if (!success) return BadRequest("Não foi possível solicitar o estorno.");
            return Ok("Solicitação de estorno registrada com sucesso.");
        }
    }

}
