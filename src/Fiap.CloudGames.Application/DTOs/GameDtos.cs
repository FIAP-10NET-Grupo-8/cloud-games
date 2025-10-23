using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.DTOs
{
    /// <summary>
    /// DTO para o fluxo "Cadastrar Jogo".
    /// (Representa os "Detalhes de um novo jogo")
    /// </summary>
    public class GameDtos
    {
        public record CreateGameDto
        (
            [Required][MaxLength(255)] string Title,
            [MaxLength(1000)] string? Description,
            [Required] decimal Price,
            DateTime ReleaseDate,
            [Required][MaxLength(100)] string Developer,
            [Required][MaxLength(100)] string Publisher,
            string? Genre,
            string? Platforms
        );

        /// <summary>
        /// DTO para o fluxo "Atualizar Jogo".
        /// (Representa as "Informações do jogo existente")
        /// </summary>
        public record UpdateGameDto
        (
            [Required][MaxLength(255)] string Title,
            [MaxLength(1000)] string? Description,
            [Required] decimal Price,
            DateTime ReleaseDate,
            [Required][MaxLength(100)] string Developer,
            [Required][MaxLength(100)] string Publisher,
            string? Genre,
            string? Platforms
        );
    }
}
