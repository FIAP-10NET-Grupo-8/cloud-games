namespace Fiap.CloudGames.Domain.UserGamesLibrary.ValueObjects;

/// <summary>
/// Filtros e paginação para consulta da biblioteca no repositório de domínio.
/// (Mantém o domínio independente de DTOs/transport models.)
/// </summary>
public sealed class LibraryFilter
{
    public string? Search { get; init; }
    public string? Genre { get; init; }
    public string? Developer { get; init; }
    public string? Publisher { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public string? SortBy { get; init; } // "title", "genre", "developer", "publisher"
    public bool Desc { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
