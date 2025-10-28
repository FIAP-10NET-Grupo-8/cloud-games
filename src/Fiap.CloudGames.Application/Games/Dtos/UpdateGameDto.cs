namespace Fiap.CloudGames.Application.Games.Dtos;

/// <summary>DTO usado para atualização completa (PUT) de um jogo.</summary>
/// <param name="Title">Título do jogo.</param>
/// <param name="Description">Descrição opcional.</param>
/// <param name="Price">Preço atual.</param>
/// <param name="ReleaseDate">Data de lançamento.</param>
/// <param name="Developer">Estúdio/desenvolvedor.</param>
/// <param name="Publisher">Publicadora.</param>
/// <param name="Genre">Gênero opcional.</param>
/// <param name="Platforms">Plataformas alvo (texto livre) opcional.</param>
/// <param name="Active">Se o jogo está ativo no catálogo.</param>
public record UpdateGameDto(
    string Title,
    string? Description,
    decimal Price,
    DateTime ReleaseDate,
    string Developer,
    string Publisher,
    string? Genre,
    string? Platforms,
    bool Active
);