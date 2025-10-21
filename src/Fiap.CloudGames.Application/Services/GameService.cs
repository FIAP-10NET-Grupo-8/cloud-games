using Fiap.CloudGames.Application.Interfaces;
using Fiap.CloudGames.Domain.Entities;
using Fiap.CloudGames.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fiap.CloudGames.Application.DTOs.GameDtos;

namespace Fiap.CloudGames.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        // Construtor para injeção de dependência
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // Fluxo: "Cadastrar Jogo"
        public async Task<Game> CreateGameAsync(CreateGameDto gameDto)
        {
            var game = new Game
            {
                Title = gameDto.Title,
                Description = gameDto.Description,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate,
                Developer = gameDto.Developer,
                Publisher = gameDto.Publisher,
                Genre = gameDto.Genre,
                Platforms = gameDto.Platforms
            };

            await _gameRepository.AddAsync(game);
            return game; // Retorna a entidade criada (com o ID)
        }


        // Fluxo: "Pesquisar/Listar Jogos"
        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _gameRepository.GetAllAsync();
        }

        // Fluxo: "Buscar Jogo"
        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _gameRepository.GetByIdAsync(id);
        }

        // Fluxo: "Atualizar Jogo"
        public async Task<bool> UpdateGameAsync(int id, UpdateGameDto gameDto)
        {
            var existingGame = await _gameRepository.GetByIdAsync(id);
            if (existingGame == null)
            {
                return false; // Jogo não encontrado
            }

            existingGame.Title = gameDto.Title;
            existingGame.Description = gameDto.Description;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Developer = gameDto.Developer;
            existingGame.Publisher = gameDto.Publisher;
            existingGame.Genre = gameDto.Genre;
            existingGame.Platforms = gameDto.Platforms;

            await _gameRepository.UpdateAsync(existingGame);
            return true;
        }

        // Fluxo: "Excluir Jogo"
        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return false; // Jogo não encontrado
            }

            await _gameRepository.DeleteAsync(game);
            return true;
        }
    }
}
