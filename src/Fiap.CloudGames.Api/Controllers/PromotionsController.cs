using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.Promotions.Dtos;
using Fiap.CloudGames.Application.Promotions.Services;
using Fiap.CloudGames.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de promoções.
/// </summary>
/// <param name="service"></param>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(Roles = nameof(UserRole.Administrator))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
public class PromotionsController(IPromotionService service) : ControllerBase
{
	private readonly IPromotionService _service = service;

	/// <summary>
	/// Lista todas as promoções.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<PromotionDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var promotions = await _service.GetAllAsync(cancellationToken);
		return Ok(promotions);
	}

	/// <summary>
	/// Obtém uma promoção pelo identificador.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(PromotionDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
	{
		var p = await _service.GetByIdAsync(id, cancellationToken);
		if (p == null) return NotFound(BasicResult.NotFound("Promoção não encontrada."));
		return Ok(p);
	}

	/// <summary>
	/// Cria uma nova promoção.
	/// </summary>
	/// <param name="dto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost]
	[ProducesResponseType(typeof(PromotionDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Create([FromBody] CreatePromotionDto dto, CancellationToken cancellationToken)
	{
		var created = await _service.CreateAsync(dto, cancellationToken);
		return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
	}

	/// <summary>
	/// Atualiza uma promoção existente.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="dto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePromotionDto dto, CancellationToken cancellationToken)
	{
		var ok = await _service.UpdateAsync(id, dto, cancellationToken);
		if (!ok) return NotFound(BasicResult.NotFound("Promoção não encontrada."));
		return NoContent();
	}

	/// <summary>
	/// Desativa uma promoção.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
	{
		var ok = await _service.DeactivateAsync(id, cancellationToken);
		if (!ok) return NotFound(BasicResult.NotFound("Promoção não encontrada."));
		return NoContent();
	}
}
