using Fiap.CloudGames.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.Interfaces
{
    /// <summary>
    /// Contrato para o repositório de dados da entidade Game.
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Busca um jogo pelo seu Id.
        /// </summary>
        Task<Game?> GetByIdAsync(int id);

        /// <summary>
        /// Busca todos os jogos cadastrados.
        /// </summary>
        Task<IEnumerable<Game>> GetAllAsync();

        /// <summary>
        /// Adiciona um novo Jogo.
        /// </summary>
        Task AddAsync(Game game);

        /// <summary>
        /// Atualiza um Jogo existente.
        /// </summary>
        Task UpdateAsync(Game game);

        /// <summary>
        /// Remove um Jogo.
        /// </summary>
        Task DeleteAsync(Game game);
    }
}
