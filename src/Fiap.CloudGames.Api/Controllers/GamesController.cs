using Fiap.CloudGames.Application.Games.Dtos;
using Fiap.CloudGames.Application.Games.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints para gerenciamento de jogos.
/// </summary>
/// <param name="gameService">Serviço de jogos utilizado pelos endpoints.</param>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public sealed class GamesController(IGameService gameService) : ControllerBase
{
    private readonly IGameService _gameService = gameService;

    /// <summary>
    /// Lista todos os jogos.
    /// </summary>
    /// <param name="ct">Token de cancelamento da requisição.</param>
    /// <returns>Lista de jogos.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<GameListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GameListItemDto>>> GetAll(CancellationToken ct)
    {
        var games = await _gameService.GetAllAsync(ct);
        return Ok(games);
    }

    /// <summary>
    /// Busca um jogo pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do jogo (GUID).</param>
    /// <param name="ct">Token de cancelamento da requisição.</param>
    /// <returns>Detalhes do jogo.</returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameDto>> GetById(Guid id, CancellationToken ct)
    {
        var game = await _gameService.GetByIdAsync(id, ct);
        if (game is null) return NotFound(new { message = "Jogo não encontrado." });
        return Ok(game);
    }

    /// <summary>
    /// Cadastra um novo jogo.
    /// </summary>
    /// <param name="dto">Dados para criação do jogo.</param>
    /// <param name="ct">Token de cancelamento da requisição.</param>
    /// <returns>Jogo criado.</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(GameDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GameDto>> Create([FromBody] CreateGameDto dto, CancellationToken ct)
    {
        var created = await _gameService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza um jogo existente.
    /// </summary>
    /// <param name="id">Identificador do jogo (GUID).</param>
    /// <param name="dto">Dados a serem atualizados.</param>
    /// <param name="ct">Token de cancelamento da requisição.</param>
    /// <returns>Sem conteúdo em caso de sucesso.</returns>
    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGameDto dto, CancellationToken ct)
    {
        var ok = await _gameService.UpdateAsync(id, dto, ct);
        if (!ok) return NotFound(new { message = "Jogo não encontrado." });
        return NoContent();
    }

    /// <summary>
    /// Exclui um jogo.
    /// </summary>
    /// <param name="id">Identificador do jogo (GUID).</param>
    /// <param name="ct">Token de cancelamento da requisição.</param>
    /// <returns>Sem conteúdo em caso de sucesso.</returns>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _gameService.DeleteAsync(id, ct);
        if (!ok) return NotFound(new { message = "Jogo não encontrado." });
        return NoContent();
    }
}