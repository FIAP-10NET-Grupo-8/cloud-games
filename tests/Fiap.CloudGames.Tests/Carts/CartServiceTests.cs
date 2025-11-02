using System.Security.Claims;
using Fiap.CloudGames.Application.Carts.Dtos;
using Fiap.CloudGames.Application.Carts.Services;
using Fiap.CloudGames.Domain.Carts.Entities;
using Fiap.CloudGames.Domain.Carts.Repositories;
using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Games.Repositories;
using Moq;

namespace Fiap.CloudGames.Tests.Carts;

public class CartServiceTests
{
    private static ClaimsPrincipal MakeUser(Guid? id = null, string email = "jose.silva@cloudgames.dev")
    {
        var uid = id ?? Guid.NewGuid();
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, uid.ToString()),
            new Claim(ClaimTypes.Email, email)
        };
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }

    private static ClaimsPrincipal MakeUser(out Guid uid, string email = "user@cloudgames.dev")
    {
        uid = Guid.NewGuid();
        return MakeUser(uid, email);
    }

    private static Game MakeGame(string title = "Clean Code: O jogo", decimal price = 100m)
    {
        return Game.Create(
            title: title,
            description: null,
            price: price,
            releaseDate: DateTime.UtcNow.Date,
            developer: "CloudGames Studio",
            publisher: "CloudGames Publishing",
            genre: "Educational",
            platforms: "PC"
        );
    }

    [Fact]
    public async Task GetMine_NoCart_CreatesAndReturns()
    {
        var repo = new Mock<ICartRepository>(MockBehavior.Strict);
        var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
        var user = MakeUser(out Guid uid);

        repo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cart?)null);
        repo.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var svc = new CartService(repo.Object, gameRepo.Object);

        var dto = await svc.GetMineAsync(user, default);

        Assert.Equal(uid, dto.UserId);
        Assert.Equal(0m, dto.Total);
        repo.Verify(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddItem_NewGame_AddsAndPersists()
    {
        var repo = new Mock<ICartRepository>(MockBehavior.Strict);
        var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
        var user = MakeUser(out Guid uid);

        var cart = Cart.Create(uid, "joao@cloudgames.dev");
        var game = MakeGame("Clean Code: O jogo", 100m);

        repo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);
        gameRepo.Setup(g => g.GetByIdAsync(game.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(game);
        repo.Setup(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var svc = new CartService(repo.Object, gameRepo.Object);

        var result = await svc.AddItemAsync(new AddCartItemDto(game.Id), idempotencyKey: null, user, default);

        Assert.Single(result.Items);
        Assert.Equal(100m, result.Total);
        repo.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddItem_Duplicate_IsIdempotent_NoUpdate()
    {
        var repo = new Mock<ICartRepository>(MockBehavior.Strict);
        var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
        var user = MakeUser(out Guid uid);

        var cart = Cart.Create(uid, "maria@cloudgames.dev");
        var gId = Guid.NewGuid();
        cart.AddItem(gId, "Kubernetes Tycoon", 100m);

        repo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        var svc = new CartService(repo.Object, gameRepo.Object);

        var result = await svc.AddItemAsync(new AddCartItemDto(gId), null, user, default);

        Assert.Single(result.Items);
        Assert.Equal(100m, result.Total);
        repo.Verify(r => r.UpdateAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RemoveItem_NoCart_CreatesEmptyAndReturns()
    {
        var repo = new Mock<ICartRepository>(MockBehavior.Strict);
        var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
        var user = MakeUser(out Guid uid);

        repo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cart?)null);
        repo.Setup(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var svc = new CartService(repo.Object, gameRepo.Object);

        var result = await svc.RemoveItemAsync(Guid.NewGuid(), user, default);

        Assert.Empty(result.Items);
        Assert.Equal(0m, result.Total);
        repo.Verify(r => r.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Clear_WhenHasItems_PersistsAndResets()
    {
        var repo = new Mock<ICartRepository>(MockBehavior.Strict);
        var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);
        var user = MakeUser(out Guid uid);

        var cart = Cart.Create(uid, "user@cloudgames.dev");
        cart.AddItem(Guid.NewGuid(), "DDD Adventure", 70m, 20m);

        repo.Setup(r => r.GetByUserIdAsync(uid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);
        repo.Setup(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var svc = new CartService(repo.Object, gameRepo.Object);

        var result = await svc.ClearAsync(user, default);

        Assert.Empty(result.Items);
        Assert.Equal(0m, result.Total);
        repo.Verify(r => r.UpdateAsync(cart, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_MapsSummaries()
    {
        var repo = new Mock<ICartRepository>(MockBehavior.Strict);
        var gameRepo = new Mock<IGameRepository>(MockBehavior.Strict);

        var c1 = Cart.Create(Guid.NewGuid(), "a@dev");
        c1.AddItem(Guid.NewGuid(), "G1", 10m);
        var c2 = Cart.Create(Guid.NewGuid(), "b@dev");

        repo.Setup(r => r.QueryAllAsync(1, 50, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Cart> { c1, c2 }, 2));

        var svc = new CartService(repo.Object, gameRepo.Object);

        var page = await svc.GetAllAsync(1, 50, default);

        Assert.Equal(2, page.TotalItems);
        Assert.Equal(2, page.Items.Count);
        Assert.Contains(page.Items, x => x.ItemsCount == 1 && x.Total == 10m);
    }
}
