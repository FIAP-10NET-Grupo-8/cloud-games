using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.UserGamesLibrary.Repositories
{
    public interface IUserGameLibraryRepository
    {
        Task AddAsync(UserGameLibrary userGameLibrary);
        Task<UserGameLibrary?> GetAsync(Guid userId, Guid gameId);
        Task DeleteAsync(UserGameLibrary userGame);
        Task<IEnumerable<Game>> GetGamesByUserIdAsync(
            Guid userId,
            string? nome,
            string? categoria,
            string? distribuidora,
            string? desenvolvedora,
            DateTime? dataInicio,
            DateTime? dataFim
        );
    }
}
