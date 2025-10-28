using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.DTOs
{
    /// <summary>
    /// Dto para o fluxo "Solicitar Estorno".
    /// </summary>
    public class RefundRequestDto
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
    }
}
