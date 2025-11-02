using Fiap.CloudGames.Api.Controllers;
using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
using Fiap.CloudGames.Application.UserGamesLibrary.Services;
using Fiap.CloudGames.Domain.Games.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Tests.UserGamesLibrary
{
    public class LibraryControllerTests
    {
        private readonly Mock<ILibraryService> _mockService;
        private readonly LibraryController _controller;
        private readonly Guid _testUserId;

        public LibraryControllerTests()
        {
            _mockService = new Mock<ILibraryService>();
            _testUserId = Guid.NewGuid();

            var userClaims = new Claim[] { new Claim("userId", _testUserId.ToString()) };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "TestAuth"));
            _controller = new LibraryController(_mockService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = userPrincipal }
                }
            };
        }

        [Fact]
        public async Task GetMyLibrary_QuandoChamado_DeveRetornarOkComListaDeJogos()
        {
            var queryParams = new LibraryQueryDto();
            var jogoDeTeste = Game.Create(
                    title: "Test Game",
                    description: "Descrição de teste",
                    price: 19.99m,
                    releaseDate: DateTime.UtcNow.AddYears(-1),
                    developer: "Dev Teste",
                    publisher: "Pub Teste",
                    genre: "RPG",
                    platforms: "PC"
                );

            var listaDeJogos = new List<Game> { jogoDeTeste };

            _mockService.Setup(s => s.GetGamesFromLibraryAsync(_testUserId, queryParams))
                .ReturnsAsync(listaDeJogos);

            var resultado = await _controller.GetMyLibrary(queryParams);

            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(listaDeJogos);
        }

        [Fact]
        public async Task PurchaseGame_QuandoSucesso_DeveRetornarOk()
        {
            var gameId = Guid.NewGuid();
            _mockService.Setup(s => s.AddGameToLibraryAsync(_testUserId, gameId))
                .ReturnsAsync(true);

            var resultado = await _controller.PurchaseGame(gameId);

            resultado.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PurchaseGame_QuandoJogoJaExiste_DeveRetornarConflict()
        {
            var gameId = Guid.NewGuid();
            _mockService.Setup(s => s.AddGameToLibraryAsync(_testUserId, gameId))
                .ReturnsAsync(false);

            var resultado = await _controller.PurchaseGame(gameId);

            resultado.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        public async Task RefundGame_QuandoSucesso_DeveRetornarNoContent()
        {
            var gameId = Guid.NewGuid();
            _mockService.Setup(s => s.RemoveGameFromLibraryAsync(_testUserId, gameId))
                .ReturnsAsync(true);

            var resultado = await _controller.RefundGame(gameId);

            resultado.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task RefundGame_QuandoJogoNaoEncontrado_DeveRetornarNotFound()
        {
            var gameId = Guid.NewGuid();
            _mockService.Setup(s => s.RemoveGameFromLibraryAsync(_testUserId, gameId))
                .ReturnsAsync(false);

            var resultado = await _controller.RefundGame(gameId);

            resultado.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}