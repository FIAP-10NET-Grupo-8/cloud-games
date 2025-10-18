using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.DTOs
{
    public class GameDtos
    {
        public class CreateGameDto
        {
            public string Title { get; set; }
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public DateTime ReleaseDate { get; set; }
            public string Publisher { get; set; }
            public string? Genre { get; set; }
            public string? Platforms { get; set; }
        }
    }
}
