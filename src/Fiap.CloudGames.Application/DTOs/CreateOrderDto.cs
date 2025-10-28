using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.DTOs
{
    /// <summary>
    /// DTO para o Criação de Pedidos.   
    /// </summary>
    public class CreateOrderDto
    {
        public Guid PlayerId { get; set; }
        public decimal TotalValue { get; set; }
        public string PaymentTransactionId { get; set; }
    }
}
