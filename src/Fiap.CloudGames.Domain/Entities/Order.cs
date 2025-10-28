using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.Entities
{
    /// <summary>
    /// Entidade que vai representar a tabela 'order' (pedido) no banco.
    /// </summary>
    public class Order
    {
        [Key]
        public Guid Id { get; set; } //nao sei se faz sentido ser um id ou um guid....

        [Required]        
        public Guid PlayerId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        [Required]        
        public string Status { get; set; } // Pago, cancelado, aguardando pagamento, etc.
               
        public bool RefundRequested { get; set; }

        public DateTime RefundRequestDate { get; set; }

        public DateTime RefundDate { get; set; }
        
        public string PaymentTransactionId { get; set; }

        public bool PaymentConfirmed { get; set; }

        //Controle Interno

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
