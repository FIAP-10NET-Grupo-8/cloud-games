namespace Fiap.CloudGames.Application.UserGamesLibrary.Dtos;

/// <summary>
/// Representa um jogo que o usuário possui em sua biblioteca (projeção para resposta da API).
/// </summary>
public sealed record LibraryGameDto(
    Guid GameId,
    string Title,
    string? Genre,
    string? Developer,
    string? Publisher
);
