using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
using Fiap.CloudGames.Application.UserGamesLibrary.Services;
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
    /// Tenta obter o GUID do usuário a partir do token (claim <c>userId</c>).
    /// </summary>
    private bool TryGetUserId(out Guid userId)
    {
        userId = Guid.Empty;
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
        return userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out userId);
    }

    /// <summary>
    /// Fluxo: "Consultar Jogos da Biblioteca" com filtros.
    /// </summary>
    [HttpGet("my-games")]
    [ProducesResponseType(typeof(PagedResult<LibraryGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyLibrary([FromQuery] LibraryListRequest request, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(Problem(title: "ID do usuário não encontrado no token.", statusCode: StatusCodes.Status401Unauthorized));

        var page = await _libraryService.GetGamesFromLibraryAsync(userId, request, ct);
        return Ok(page);
    }

    /// <summary>
    /// Simula a compra/liberação de um jogo na biblioteca do usuário autenticado.
    /// </summary>
    /// <param name="gameId">Identificador do jogo (GUID).</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpPost("buy/{gameId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PurchaseGame(Guid gameId, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(Problem(title: "ID do usuário não encontrado no token.", statusCode: StatusCodes.Status401Unauthorized));

        var result = await _libraryService.AddGameToLibraryAsync(userId, gameId, ct);

        return result switch
        {
            AddGameResult.Added => Ok(new { message = "Jogo adicionado à biblioteca com sucesso." }),
            AddGameResult.AlreadyOwned => Conflict(Problem(title: "Este jogo já está na sua biblioteca.", statusCode: StatusCodes.Status409Conflict)),
            AddGameResult.GameNotFound => NotFound(Problem(title: "Jogo não encontrado.", statusCode: StatusCodes.Status404NotFound)),
            _ => Problem(title: "Não foi possível processar a solicitação.", statusCode: StatusCodes.Status400BadRequest)
        };
    }

    /// <summary>
    /// Fluxo: "Estorno Realizado" / "Remover Jogo" da biblioteca.
    /// </summary>
    /// <param name="gameId">Identificador do jogo (GUID).</param>
    /// <param name="ct">Token de cancelamento.</param>
    [HttpDelete("remove/{gameId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefundGame(Guid gameId, CancellationToken ct)
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized(Problem(title: "ID do usuário não encontrado no token.", statusCode: StatusCodes.Status401Unauthorized));

        var removed = await _libraryService.RemoveGameFromLibraryAsync(userId, gameId, ct);
        if (!removed)
            return NotFound(Problem(title: "Este jogo não foi encontrado na sua biblioteca.", statusCode: StatusCodes.Status404NotFound));

        return NoContent();
    }
}
