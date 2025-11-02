using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Application.UserGamesLibrary.Dtos
{
    /// <summary>
    /// DTO para os parâmetros de filtro da consulta à biblioteca.
    /// </summary>
    public class LibraryQueryDto
    {
        public string? Nome { get; set; }
        public string? Categoria { get; set; }
        public string? Distribuidora { get; set; }
        public string? Desenvolvedora { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
    }
}
