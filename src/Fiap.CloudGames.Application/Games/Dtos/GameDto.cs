namespace Fiap.CloudGames.Application.Games.Dtos;

/// <summary>DTO público que representa um jogo para consumo por APIs.</summary>
/// <param name="Id">Identificador único do jogo.</param>
/// <param name="Title">Título do jogo.</param>
/// <param name="Description">Descrição opcional.</param>
/// <param name="Price">Preço atual.</param>
/// <param name="ReleaseDate">Data de lançamento.</param>
/// <param name="Developer">Estúdio/desenvolvedor.</param>
/// <param name="Publisher">Publicadora.</param>
/// <param name="Genre">Gênero opcional.</param>
/// <param name="Platforms">Plataformas alvo (texto livre) opcional.</param>
/// <param name="Active">Se o jogo está ativo no catálogo.</param>
/// <param name="CreatedAt">Data de criação do registro.</param>
/// <param name="UpdatedAt">Última atualização (se houver).</param>
public record GameDto(
    Guid Id,
    string Title,
    string? Description,
    decimal Price,
    DateTime ReleaseDate,
    string Developer,
    string Publisher,
    string? Genre,
    string? Platforms,
    bool Active,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);