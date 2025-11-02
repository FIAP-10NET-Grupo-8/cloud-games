using Fiap.CloudGames.Application.UserGamesLibrary.Dtos;
using Fiap.CloudGames.Application.UserGamesLibrary.Services;
using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.UserGamesLibrary.Repositories;
using FluentAssertions;
using UserGamesLibraryEntity = Fiap.CloudGames.Domain.UserGamesLibrary.Entities.UserGameLibrary;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Tests.UserGamesLibrary
{
    public class LibraryServiceTests
    {
        private readonly Mock<IUserGameLibraryRepository> _mockRepo;
        private readonly ILibraryService _sut;
        private readonly Guid _testUserId;
        private readonly Guid _testGameId;

        public LibraryServiceTests()
        {
            _mockRepo = new Mock<IUserGameLibraryRepository>();
            _sut = new LibraryService(_mockRepo.Object);
            _testUserId = Guid.NewGuid();
            _testGameId = Guid.NewGuid();
        }

        [Fact]
        public async Task AddGameToLibraryAsync_QuandoJogoNaoExiste_DeveAdicionarComSucesso()
        {
            _mockRepo.Setup(r => r.GetAsync(_testUserId, _testGameId))
                .ReturnsAsync((UserGamesLibraryEntity)null);

            var resultado = await _sut.AddGameToLibraryAsync(_testUserId, _testGameId);

            resultado.Should().BeTrue();
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<UserGamesLibraryEntity>()), Times.Once);
        }

        [Fact]
        public async Task AddGameToLibraryAsync_QuandoJogoJaExiste_DeveRetornarFalso()
        {
            _mockRepo.Setup(r => r.GetAsync(_testUserId, _testGameId))
                .ReturnsAsync(new UserGamesLibraryEntity());

            var resultado = await _sut.AddGameToLibraryAsync(_testUserId, _testGameId);

            resultado.Should().BeFalse();
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<UserGamesLibraryEntity>()), Times.Never);
        }

        [Fact]
        public async Task RemoveGameFromLibraryAsync_QuandoJogoExiste_DeveRemoverComSucesso()
        {
            var jogoNaBiblioteca = new UserGamesLibraryEntity { UserId = _testUserId, GameId = _testGameId };
            _mockRepo.Setup(r => r.GetAsync(_testUserId, _testGameId))
                .ReturnsAsync(jogoNaBiblioteca);

            var resultado = await _sut.RemoveGameFromLibraryAsync(_testUserId, _testGameId);

            resultado.Should().BeTrue();
            _mockRepo.Verify(r => r.DeleteAsync(jogoNaBiblioteca), Times.Once);
        }

        [Fact]
        public async Task RemoveGameFromLibraryAsync_QuandoJogoNaoExiste_DeveRetornarFalso()
        {
            _mockRepo.Setup(r => r.GetAsync(_testUserId, _testGameId))
                .ReturnsAsync((UserGamesLibraryEntity)null);

            var resultado = await _sut.RemoveGameFromLibraryAsync(_testUserId, _testGameId);

            resultado.Should().BeFalse();
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<UserGamesLibraryEntity>()), Times.Never);
        }

        [Fact]
        public async Task GetGamesFromLibraryAsync_DeveChamarRepositorio_ComParametrosTraduzidos()
        {
            var queryParams = new LibraryQueryDto { Categoria = "RPG" };

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

            var listaDeJogosEsperada = new List<Game> { jogoDeTeste };

            _mockRepo.Setup(r => r.GetGamesByUserIdAsync(
                    _testUserId,
                    null, "RPG", null, null, null, null
                ))
                .ReturnsAsync(listaDeJogosEsperada);

            var resultado = await _sut.GetGamesFromLibraryAsync(_testUserId, queryParams);

            resultado.Should().HaveCount(1);
            resultado.First().Title.Should().Be("Test Game");
            _mockRepo.Verify();
        }
    }
}
