using Fiap.CloudGames.Domain.Games.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.UserGamesLibrary.Services
{
    public interface ILibraryService
    {
        /// <summary>
        /// Libera um jogo para a biblioteca de um usuário 
        /// </summary>
        Task<bool> AddGameToLibraryAsync(Guid userId, Guid gameId);

        /// <summary>
        /// Consulta os jogos de um usuário
        /// </summary>
        Task<IEnumerable<Game>> GetGamesFromLibraryAsync(Guid userId);
    }
}
