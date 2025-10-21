using Fiap.CloudGames.Application.Interfaces;
using Fiap.CloudGames.Domain.Entities;
using Fiap.CloudGames.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Fiap.CloudGames.Application.DTOs.GameDtos;

namespace Fiap.CloudGames.Api.Controllers
{
    //[Authorize] // Exige autenticação em todos os endpoints
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Endpoint para o fluxo "Pesquisar/Listar Jogos"
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Game>), 200)]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        /// <summary>
        /// Endpoint para o fluxo "Buscar Jogo"
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Game), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        /// <summary>
        /// Endpoint para o fluxo "Cadastrar Jogo"
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Game), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto gameDto)
        {
            var newGame = await _gameService.CreateGameAsync(gameDto);
            // Retorna 201 Created com a localização do novo recurso
            return CreatedAtAction(nameof(GetGameById), new { id = newGame.Id }, newGame);
        }

        /// <summary>
        /// Endpoint para o fluxo "Atualizar Jogo"
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] UpdateGameDto gameDto)
        {
            var success = await _gameService.UpdateGameAsync(id, gameDto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content (sucesso)
        }

        /// <summary>
        /// Endpoint para o fluxo "Excluir Jogo"
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var success = await _gameService.DeleteGameAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); // 204 No Content (sucesso)
        }
    }
}
