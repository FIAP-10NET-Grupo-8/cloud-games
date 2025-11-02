using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;
using Fiap.CloudGames.Domain.Users.Entities;
using Fiap.CloudGames.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Infrastructure.UserGamesLibrary.Repositories
{
    public class UserGameLibraryRepository : IUserGameLibraryRepository
    {
        private readonly AppDbContext _context;
        public UserGameLibraryRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(UserGameLibrary userGame)
        {
            await _context.UserGameLibrary.AddAsync(userGame);
            await _context.SaveChangesAsync();
        }

        public async Task<UserGameLibrary?> GetAsync(Guid userId, Guid gameId)
        {
            return await _context.UserGameLibrary.FindAsync(userId, gameId);
        }

        public async Task DeleteAsync(UserGameLibrary userGame)
        {
            _context.UserGameLibrary.Remove(userGame);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByUserIdAsync(
                    Guid userId,
                    string? nome,
                    string? categoria,
                    string? distribuidora,
                    string? desenvolvedora,
                    DateTime? dataInicio,
                    DateTime? dataFim)
        {
            var query = _context.UserGameLibrary
                .Where(ug => ug.UserId == userId)
                .Select(ug => ug.Game);

            // Lógica de filtro dinâmico
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(g => g.Title.Contains(nome));

            if (!string.IsNullOrEmpty(categoria))
                query = query.Where(g => g.Genre != null && g.Genre.Contains(categoria));

            if (!string.IsNullOrEmpty(desenvolvedora))
                query = query.Where(g => g.Developer.Contains(desenvolvedora));

            if (!string.IsNullOrEmpty(distribuidora))
                query = query.Where(g => g.Publisher.Contains(distribuidora));

            if (dataInicio.HasValue)
                query = query.Where(g => g.ReleaseDate >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(g => g.ReleaseDate <= dataFim.Value);

            return await query.ToListAsync();
        }
    }
}
