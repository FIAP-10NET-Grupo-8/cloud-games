using Fiap.CloudGames.Domain.Carts.Entities;

namespace Fiap.CloudGames.Tests.Carts;

public class CartTests
{
    [Fact]
    public void Create_Valid_SetsDefaults()
    {
        var uid = Guid.NewGuid();
        var cart = Cart.Create(uid, "jose.silva@cloudgames.dev");

        Assert.NotEqual(Guid.Empty, cart.Id);
        Assert.Equal(uid, cart.UserId);
        Assert.Equal("jose.silva@cloudgames.dev", cart.UserEmail);
        Assert.Equal(0m, cart.TotalValue);
        Assert.Empty(cart.Items);
        Assert.NotEqual(default, cart.CreatedAt);
        Assert.Null(cart.UpdatedAt);
    }

    [Fact]
    public void Create_InvalidUserId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Cart.Create(Guid.Empty, "user@cloudgames.dev"));
    }

    [Fact]
    public void AddItem_WhenNotExists_AddsOnceAndUpdatesTotal_AndUpdatedAt()
    {
        var cart = Cart.Create(Guid.NewGuid(), "ana.maria@cloudgames.dev");
        var gid = Guid.NewGuid();

        cart.AddItem(gid, "Kubernetes Tycoon", 100m);

        Assert.Single(cart.Items);
        Assert.Equal(100m, cart.TotalValue);
        Assert.NotNull(cart.UpdatedAt);
    }

    [Fact]
    public void AddItem_WhenDuplicate_IgnoresAndKeepsTotal()
    {
        var cart = Cart.Create(Guid.NewGuid(), "ana.maria@cloudgames.dev");
        var gid = Guid.NewGuid();

        cart.AddItem(gid, "Kubernetes Tycoon", 100m);
        var updatedAt1 = cart.UpdatedAt;

        cart.AddItem(gid, "Kubernetes Tycoon", 100m);

        Assert.Single(cart.Items);
        Assert.Equal(100m, cart.TotalValue);
        Assert.Equal(updatedAt1, cart.UpdatedAt);
    }

    [Fact]
    public void AddItem_WithDiscount_AppliesToTotal()
    {
        var cart = Cart.Create(Guid.NewGuid(), "jose.silva@cloudgames.dev");

        cart.AddItem(Guid.NewGuid(), "Clean Code: O jogo", 100m, discount: 10m);
        cart.AddItem(Guid.NewGuid(), "Simulador de Refatoração", 50m, discount: 0m);

        Assert.Equal(140m, cart.TotalValue);
    }

    [Fact]
    public void RemoveItem_WhenExists_RemovesAndUpdatesTotal()
    {
        var cart = Cart.Create(Guid.NewGuid(), "jose.silva@cloudgames.dev");
        var gid = Guid.NewGuid();
        cart.AddItem(gid, "Clean Architecture Quest", 100m);
        cart.AddItem(Guid.NewGuid(), "Design Patterns Builder", 50m);

        cart.RemoveItem(gid);

        Assert.Single(cart.Items);
        Assert.Equal(50m, cart.TotalValue);
        Assert.NotNull(cart.UpdatedAt);
    }

    [Fact]
    public void RemoveItem_WhenNotExists_NoOp()
    {
        var cart = Cart.Create(Guid.NewGuid(), "david.silva@cloudgames.dev");
        cart.AddItem(Guid.NewGuid(), "Event Sourcing Arena", 100m);

        var updatedAt1 = cart.UpdatedAt;
        cart.RemoveItem(Guid.NewGuid());

        Assert.Single(cart.Items);
        Assert.Equal(100m, cart.TotalValue);
        Assert.Equal(updatedAt1, cart.UpdatedAt);
    }

    [Fact]
    public void Clear_WhenHasItems_EmptiesAndResetsTotal()
    {
        var cart = Cart.Create(Guid.NewGuid(), "ana.maria@cloudgames.dev");
        cart.AddItem(Guid.NewGuid(), "CQRS Runner", 100m);
        cart.AddItem(Guid.NewGuid(), "DDD Adventure", 70m, discount: 20m);

        cart.Clear();

        Assert.Empty(cart.Items);
        Assert.Equal(0m, cart.TotalValue);
        Assert.NotNull(cart.UpdatedAt);
    }

    [Fact]
    public void Clear_WhenEmpty_NoOp()
    {
        var cart = Cart.Create(Guid.NewGuid(), "ana.maria@cloudgames.dev");
        var updatedAt1 = cart.UpdatedAt;

        cart.Clear();

        Assert.Empty(cart.Items);
        Assert.Equal(0m, cart.TotalValue);
        Assert.Equal(updatedAt1, cart.UpdatedAt);
    }
}
