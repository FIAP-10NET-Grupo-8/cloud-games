using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
using Fiap.CloudGames.Application.UserGamesLibrary.Services;
using Fiap.CloudGames.Domain.Games.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Fiap.CloudGames.Api.Controllers;

/// <summary>
/// Endpoints da biblioteca do usuário (consulta e compra/liberação).
/// </summary>
/// <param name="libraryService">Serviço de biblioteca responsável pelas operações de biblioteca.</param>
[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize] // exige usuário autenticado
public sealed class LibraryController(ILibraryService libraryService) : ControllerBase
{
    private readonly ILibraryService _libraryService = libraryService;

    /// <summary>
    /// Obtém o GUID do usuário a partir do token (claim <c>userId</c>).
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">
    /// Lançada quando a claim <c>userId</c> não existe ou não é um GUID válido.
    /// </exception>
    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("ID do usuário (GUID) não encontrado no token.");
        return userId;
    }

    /// <summary>
    /// Fluxo: "Consultar Jogos da Biblioteca" com filtros
    /// </summary>
    [HttpGet("my-games")]
    [ProducesResponseType(typeof(IEnumerable<Game>), 200)]
    public async Task<IActionResult> GetMyLibrary([FromQuery] LibraryQueryDto queryParams)
    {
        var userId = GetUserIdFromToken();
        var games = await _libraryService.GetGamesFromLibraryAsync(userId, queryParams);
        return Ok(games);
    }

    /// <summary>
    /// Simula a compra/liberação de um jogo na biblioteca do usuário autenticado.
    /// </summary>
    /// <param name="gameId">Identificador do jogo (GUID).</param>
    /// <returns>Mensagem de sucesso ou conflito.</returns>
    [HttpPost("buy/{gameId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PurchaseGame(Guid gameId)
    {
        var userId = GetUserIdFromToken();

        var success = await _libraryService.AddGameToLibraryAsync(userId, gameId);
        if (!success)
            return Conflict(new { message = "Este jogo já está na sua biblioteca." });

        return Ok(new { message = "Jogo adicionado à biblioteca com sucesso." });
    }

    /// <summary>
    /// Fluxo: "Estorno Realizado" / "Remover Jogo"
    /// </summary>
    [HttpDelete("remove/{gameId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RefundGame(Guid gameId)
    {
        var userId = GetUserIdFromToken();
        var success = await _libraryService.RemoveGameFromLibraryAsync(userId, gameId);

        if (!success)
        {
            return NotFound(new { message = "Este jogo não foi encontrado na sua biblioteca." });
        }

        return NoContent();
    }
}
