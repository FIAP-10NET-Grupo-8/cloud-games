using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.DTOs
{
    /// <summary>
    /// DTO para consulta de Pedidos.
    /// </summary>
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; }
        public bool RefundRequested { get; set; }
    }
}
