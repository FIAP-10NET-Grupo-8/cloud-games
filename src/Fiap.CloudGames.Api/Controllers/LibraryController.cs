using Fiap.CloudGames.Application.UserGamesLibrary.Services;
using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Users.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.CloudGames.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        private Guid GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("ID do usuário (GUID) não encontrado no token.");
            }
            return userId;
        }

        /// <summary>
        /// Busca os jogos na biblioteca do usuário logado.
        /// </summary>
        [HttpGet("meus-jogos")]
        public async Task<IActionResult> GetMyLibrary()
        {
            var userId = GetUserIdFromToken();
            var games = await _libraryService.GetGamesFromLibraryAsync(userId);
            return Ok(games);
        }

        /// <summary>
        /// Simula a compra e liberação de um jogo na biblioteca.
        /// </summary>
        [HttpPost("comprar/{gameId:Guid}")] 
        public async Task<IActionResult> PurchaseGame(Guid gameId)
        {
            var userId = GetUserIdFromToken();

            var success = await _libraryService.AddGameToLibraryAsync(userId, gameId);

            if (!success)
            {
                return Conflict(new { message = "Este jogo já está na sua biblioteca." });
            }

            return Ok(new { message = "Jogo adicionado à biblioteca com sucesso." });
        }
    }
}
