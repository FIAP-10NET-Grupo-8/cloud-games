using Fiap.CloudGames.Domain.Entities;
using Fiap.CloudGames.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Infrastructure.Repositories
{
    /// <summary>
    /// Esta é uma implementação "fake" do IGameRepository que usa uma 
    /// lista estática em memória para simular um banco de dados.
    /// Perfeito para testes e desenvolvimento sem um banco real.
    /// </summary>
    public class InMemoryGameRepository : IGameRepository
    {
        private static readonly List<Game> _games = new List<Game>();
        private static int _nextId = 1; // Auto-incremento do ID

        public Task AddAsync(Game game)
        {
            // Simula o auto-incremento do ID
            game.Id = _nextId++;
            _games.Add(game);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Game game)
        {
            var existingGame = _games.FirstOrDefault(g => g.Id == game.Id);
            if (existingGame != null)
            {
                _games.Remove(existingGame);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Game>> GetAllAsync()
        {
            return Task.FromResult(_games.AsEnumerable());
        }

        public Task<Game?> GetByIdAsync(int id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            return Task.FromResult(game);
        }

        public Task UpdateAsync(Game game)
        {
            var existingGame = _games.FirstOrDefault(g => g.Id == game.Id);
            if (existingGame != null)
            {
                existingGame.Title = game.Title;
                existingGame.Description = game.Description;
                existingGame.Price = game.Price;
                existingGame.ReleaseDate = game.ReleaseDate;
                existingGame.Developer = game.Developer;
                existingGame.Publisher = game.Publisher;
                existingGame.Genre = game.Genre;
                existingGame.Platforms = game.Platforms;
            }
            return Task.CompletedTask;
        }
    }
}
