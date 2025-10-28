using Fiap.CloudGames.Application.Games.Dtos;
using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Games.Repositories;

namespace Fiap.CloudGames.Application.Games.Services;

public class GameService(IGameRepository repository) : IGameService
{
    private readonly IGameRepository _repository = repository;

    public async Task<IReadOnlyList<GameListItemDto>> GetAllAsync(CancellationToken ct)
    {
        var all = await _repository.GetAllAsync(ct);
        return all
            .OrderBy(g => g.Title)
            .Select(ToListItem)
            .ToList();
    }

    public async Task<GameDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var game = await _repository.GetByIdAsync(id, ct);
        return game is null ? null : ToDto(game);
    }

    public async Task<GameDto> CreateAsync(CreateGameDto dto, CancellationToken ct)
    {
        var duplicated = await _repository.ExistsByTitleAsync(dto.Title, ct);
        if (duplicated) throw new ArgumentException("Já existe um jogo com este título.");

        var entity = Game.Create(
            title: dto.Title,
            description: dto.Description,
            price: dto.Price,
            releaseDate: dto.ReleaseDate,
            developer: dto.Developer,
            publisher: dto.Publisher,
            genre: dto.Genre,
            platforms: dto.Platforms
        );

        await _repository.AddAsync(entity, ct);
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateGameDto dto, CancellationToken ct)
    {
        var current = await _repository.GetByIdAsync(id, ct);
        if (current is null) return false;

        // se o título mudou, garantir unicidade
        if (!string.Equals(current.Title, dto.Title, StringComparison.OrdinalIgnoreCase))
        {
            var duplicated = await _repository.ExistsByTitleAsync(dto.Title, ct);
            if (duplicated) throw new ArgumentException("Já existe um jogo com este título.");
        }

        current.Update(
            title: dto.Title,
            description: dto.Description,
            price: dto.Price,
            releaseDate: dto.ReleaseDate,
            developer: dto.Developer,
            publisher: dto.Publisher,
            genre: dto.Genre,
            platforms: dto.Platforms,
            active: dto.Active
        );

        await _repository.UpdateAsync(current, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var existing = await _repository.GetByIdAsync(id, ct);
        if (existing is null) return false;

        await _repository.DeleteAsync(id, ct);
        return true;
    }

    private static GameListItemDto ToListItem(Game g) =>
        new(g.Id, g.Title, g.Price, g.Active);

    private static GameDto ToDto(Game g) =>
        new(g.Id, g.Title, g.Description, g.Price, g.ReleaseDate, g.Developer, g.Publisher,
            g.Genre, g.Platforms, g.Active, g.CreatedAt, g.UpdatedAt);
}