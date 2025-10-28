using Fiap.CloudGames.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.Interfaces
{
    /// <summary>
    /// Contrato para o repositório de dados da entidade Order.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Busca um jogo pelo seu Id.
        /// </summary>
        Task<Order?> GetByIdAsync(Guid id);

        /// <summary>
        /// Busca todos os pedidos salvos.
        /// </summary>
        Task<IEnumerable<Order>> GetAllAsync(DateTime? startDate, DateTime? endDate, string status);

        /// <summary>
        /// Adiciona novo pedido.
        /// </summary>
        Task AddAsync(Order order);

        /// <summary>
        /// Atualiza um pedido existente.
        /// </summary>
        Task UpdateAsync(Order order);

        /// <summary>
        /// Remove um Pedido.
        /// </summary>
        Task DeleteAsync(Order order);
    }
}
