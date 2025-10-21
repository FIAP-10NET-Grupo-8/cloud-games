using Fiap.CloudGames.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fiap.CloudGames.Application.DTOs.GameDtos;

namespace Fiap.CloudGames.Application.Interfaces
{
    /// <summary>
    /// Contrato para os serviços e lógica de negócio de Jogos.
    /// </summary>
    public interface IGameService
    {
        /// <summary>
        /// Fluxo "Buscar Jogo"
        /// </summary>
        Task<Game?> GetGameByIdAsync(int id);

        /// <summary>
        /// Fluxo "Pesquisar/Listar Jogos"
        /// </summary>
        Task<IEnumerable<Game>> GetAllGamesAsync();

        /// <summary>
        /// Fluxo "Cadastrar Jogo"
        /// </summary>
        Task<Game> CreateGameAsync(CreateGameDto gameDto);

        /// <summary>
        /// Fluxo "Atualizar Jogo"
        /// </summary>
        Task<bool> UpdateGameAsync(int id, UpdateGameDto gameDto);

        /// <summary>
        /// Fluxo "Excluir Jogo"
        /// </summary>
        Task<bool> DeleteGameAsync(int id);
    }
}
