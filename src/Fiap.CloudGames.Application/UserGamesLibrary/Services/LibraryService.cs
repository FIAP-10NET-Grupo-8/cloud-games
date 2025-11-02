using Fiap.CloudGames.Application.Common;
using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
using Fiap.CloudGames.Domain.Games.Repositories;
using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;
using Fiap.CloudGames.Domain.UserGamesLibrary.ValueObjects;

namespace Fiap.CloudGames.Application.UserGamesLibrary.Services;

public class LibraryService : ILibraryService
{
    private readonly IUserGameLibraryRepository _libraryRepo;
    private readonly IGameRepository _gameRepo;

    public LibraryService(
        IUserGameLibraryRepository libraryRepo,
        IGameRepository gameRepo)
    {
        _libraryRepo = libraryRepo;
        _gameRepo = gameRepo;
    }

    public async Task<AddGameResult> AddGameToLibraryAsync(Guid userId, Guid gameId, CancellationToken ct)
    {
        // já possui?
        var existing = await _libraryRepo.GetAsync(userId, gameId, ct);
        if (existing is not null) return AddGameResult.AlreadyOwned;

        // jogo existe?
        var game = await _gameRepo.GetByIdAsync(gameId, ct);
        if (game is null) return AddGameResult.GameNotFound;

        // entidade tem ctor privado e setters privados -> usar factory
        var link = UserGameLibrary.Create(userId, gameId, DateTime.UtcNow);

        await _libraryRepo.AddAsync(link, ct);
        return AddGameResult.Added;
    }

    public async Task<bool> RemoveGameFromLibraryAsync(Guid userId, Guid gameId, CancellationToken ct)
    {
        var link = await _libraryRepo.GetAsync(userId, gameId, ct);
        if (link is null) return false;

        await _libraryRepo.DeleteAsync(link, ct);
        return true;
    }

    public async Task<PagedResult<LibraryGameDto>> GetGamesFromLibraryAsync(
        Guid userId,
        LibraryListRequest request,
        CancellationToken ct)
    {
        // Traduz o request (Application) para LibraryFilter (Domain)
        var filter = new LibraryFilter
        {
            Search = request.Search,
            Genre = request.Genre,
            Developer = request.Developer,
            Publisher = request.Publisher,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            SortBy = request.SortBy,
            Desc = request.Desc,
            Page = request.Page,
            PageSize = request.PageSize
        };

        // Repositório agora retorna vínculos com Include(Game)
        var (links, total) = await _libraryRepo.GetByUserIdAsync(userId, filter, ct);

        var items = links
            .Where(ug => ug.Game is not null)
            .Select(ug => new LibraryGameDto(
                ug.Game!.Id,
                ug.Game!.Title,
                ug.Game!.Genre,
                ug.Game!.Developer,
                ug.Game!.Publisher
            ))
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)Math.Clamp(request.PageSize, 1, 100));

        return new PagedResult<LibraryGameDto>(
            Items: items,
            Page: Math.Max(1, request.Page),
            PageSize: Math.Clamp(request.PageSize, 1, 100),
            TotalItems: total,
            TotalPages: totalPages
        );
    }
}
