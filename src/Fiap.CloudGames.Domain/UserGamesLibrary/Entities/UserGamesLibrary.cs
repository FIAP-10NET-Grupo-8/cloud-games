using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Domain.UserGamesLibrary.Entities
{
    /// <summary>
    /// Entidade de ligação para a relação Muitos-para-Muitos
    /// entre User e Game. Esta tabela representa a "Biblioteca".
    /// </summary>
    public class UserGameLibrary
    {
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }
    }
}
