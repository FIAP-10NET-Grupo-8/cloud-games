using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.UserGamesLibrary.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IUserGameLibraryRepository _userGameLibraryRepository;

        public LibraryService(IUserGameLibraryRepository userGameLibraryRepository)
        {
            _userGameLibraryRepository = userGameLibraryRepository;
        }

        public async Task<bool> AddGameToLibraryAsync(Guid userId, Guid gameId)
        {
            var existingEntry = await _userGameLibraryRepository.GetAsync(userId, gameId);
            if (existingEntry != null)
            {
                return false; // Jogo já está na biblioteca
            }

            var libraryEntry = new UserGameLibrary
            {
                UserId = userId,
                GameId = gameId,
                PurchaseDate = DateTime.UtcNow 
            };

            await _userGameLibraryRepository.AddAsync(libraryEntry);
            return true;
        }

        public async Task<bool> RemoveGameFromLibraryAsync(Guid userId, Guid gameId)
        {
            var gameToRemove = await _userGameLibraryRepository.GetAsync(userId, gameId);

            if (gameToRemove == null)
            {
                return false;
            }

            await _userGameLibraryRepository.DeleteAsync(gameToRemove);
            return true;
        }

        public async Task<IEnumerable<Game>> GetGamesFromLibraryAsync(Guid userId, LibraryQueryDto queryParams)
        {
            return await _userGameLibraryRepository.GetGamesByUserIdAsync(
                userId,
                queryParams.Nome,
                queryParams.Categoria,
                queryParams.Distribuidora,
                queryParams.Desenvolvedora,
                queryParams.DataInicio,
                queryParams.DataFim
            );
        }
    }
}
