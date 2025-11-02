using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
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
        /// Fluxo de Compra: Libera um jogo para a biblioteca de um usuário
        /// </summary>
        Task<bool> AddGameToLibraryAsync(Guid userId, Guid gameId);

        /// <summary>
        /// Fluxo de Estorno: Remove um jogo da biblioteca de um usuário
        /// </summary>
        Task<bool> RemoveGameFromLibraryAsync(Guid userId, Guid gameId);

        /// <summary>
        /// Fluxo de Consulta: Busca os jogos de um usuário com filtros
        /// </summary>
        Task<IEnumerable<Game>> GetGamesFromLibraryAsync(Guid userId, LibraryQueryDto queryParams);
    }
}
