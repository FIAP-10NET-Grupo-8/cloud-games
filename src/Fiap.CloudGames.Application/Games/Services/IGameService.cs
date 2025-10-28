using Fiap.CloudGames.Application.Games.Dtos;

namespace Fiap.CloudGames.Application.Games.Services;

public interface IGameService
{
    Task<IReadOnlyList<GameListItemDto>> GetAllAsync(CancellationToken ct);
    Task<GameDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, UpdateGameDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}