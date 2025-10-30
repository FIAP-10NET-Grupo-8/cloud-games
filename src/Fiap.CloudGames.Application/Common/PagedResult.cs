namespace Fiap.CloudGames.Application.Common;

/// <summary>Resultado paginado padrão.</summary>
/// <param name="Items">Itens retornados.</param>
/// <param name="Page">Página atual (1-based).</param>
/// <param name="PageSize">Tamanho da página.</param>
/// <param name="TotalItems">Total de itens na consulta.</param>
/// <param name="TotalPages">Total de páginas.</param>
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);