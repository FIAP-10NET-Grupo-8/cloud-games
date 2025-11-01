namespace Fiap.CloudGames.Application.Carts.Dtos;

/// <summary>DTO para adicionar um jogo ao carrinho.</summary>
/// <param name="GameId">Identificador do jogo.</param>
public sealed record AddCartItemDto(Guid GameId);
