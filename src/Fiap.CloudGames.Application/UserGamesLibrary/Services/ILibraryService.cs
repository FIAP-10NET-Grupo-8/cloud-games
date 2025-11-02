using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;

namespace Fiap.CloudGames.Application.UserGamesLibrary.Services;

public interface ILibraryService
{
    /// <summary>Fluxo de consulta: lista a biblioteca do usuário com filtros.</summary>
    Task<PagedResult<LibraryGameDto>> GetGamesFromLibraryAsync(
        Guid userId,
        LibraryListRequest query,
        CancellationToken ct);

    /// <summary>Fluxo de compra: libera um jogo para a biblioteca do usuário.</summary>
    Task<AddGameResult> AddGameToLibraryAsync(
        Guid userId,
        Guid gameId,
        CancellationToken ct);

    /// <summary>Fluxo de estorno: remove um jogo da biblioteca do usuário.</summary>
    Task<bool> RemoveGameFromLibraryAsync(
        Guid userId,
        Guid gameId,
        CancellationToken ct);
}
