namespace Fiap.CloudGames.Application.UserGamesLibrary.Dtos;

/// <summary>
/// Parâmetros de filtro/ordenação/paginação para consulta da biblioteca.
/// </summary>
public sealed record LibraryListRequest(
    string? Search,
    string? Genre,
    string? Developer,
    string? Publisher,
    DateTime? StartDate,
    DateTime? EndDate,
    string? SortBy,   // "title" (default), "genre", "publisher", "developer"
    bool Desc = false,
    int Page = 1,
    int PageSize = 20
);
