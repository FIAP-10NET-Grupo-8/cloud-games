using System.Net.Mime;
using Fiap.CloudGames.Application.UserGamesLibrary.Services;
using Fiap.CloudGames.Domain.Games.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    /// Busca os jogos na biblioteca do usuário autenticado.
    /// </summary>
    /// <returns>Lista de jogos da biblioteca.</returns>
    [HttpGet("meus-jogos")]
    [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Game>>> GetMyLibrary()
    {
        var userId = GetUserIdFromToken();
        var games = await _libraryService.GetGamesFromLibraryAsync(userId);
        return Ok(games);
    }

    /// <summary>
    /// Simula a compra/liberação de um jogo na biblioteca do usuário autenticado.
    /// </summary>
    /// <param name="gameId">Identificador do jogo (GUID).</param>
    /// <returns>Mensagem de sucesso ou conflito.</returns>
    [HttpPost("comprar/{gameId:guid}")]
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
}
