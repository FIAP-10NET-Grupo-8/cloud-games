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
    /// Entidade que vai representar a tabela 'Games' no banco de dados.
    /// </summary>
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Developer { get; set; }

        [Required]
        [MaxLength(100)]
        public string Publisher { get; set; }

        [MaxLength(100)]
        public string? Genre { get; set; }

        [MaxLength(255)]
        public string? Platforms { get; set; }
    }
}
