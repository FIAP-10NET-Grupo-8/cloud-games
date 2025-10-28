namespace Fiap.CloudGames.Application.Games.Dtos;

/// <summary>Item compacto para listagem de jogos.</summary>
/// <param name="Id">Identificador único do jogo.</param>
/// <param name="Title">Título do jogo.</param>
/// <param name="Price">Preço atual.</param>
/// <param name="Active">Se o jogo está ativo no catálogo.</param>
public record GameListItemDto(Guid Id, string Title, decimal Price, bool Active);