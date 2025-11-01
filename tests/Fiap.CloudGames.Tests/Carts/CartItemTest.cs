using Fiap.CloudGames.Domain.Carts.Entities;

namespace Fiap.CloudGames.Tests.Carts;

public class CartItemTests
{
    [Fact]
    public void Create_ValidItem_ComputesFinalPrice()
    {
        var gid = Guid.NewGuid();

        var item = CartItem.Create(
            gameId: gid,
            title: "Clean Code: O jogo",
            unitPrice: 100m,
            discount: 20m
        );

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal(gid, item.GameId);
        Assert.Equal("Clean Code: O jogo", item.Title);
        Assert.Equal(100m, item.UnitPrice);
        Assert.Equal(20m, item.Discount);
        Assert.Equal(80m, item.FinalPrice);
    }

    [Fact]
    public void Create_ValidItem_WithoutDiscount_DefaultsToZero()
    {
        var item = CartItem.Create(Guid.NewGuid(), "Simulador de Refatoração", 59.90m);

        Assert.Equal(0m, item.Discount);
        Assert.Equal(59.90m, item.FinalPrice);
    }

    [Fact]
    public void Create_InvalidGameId_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            CartItem.Create(Guid.Empty, "Any Title", 10m));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidTitle_Throws(string title)
    {
        Assert.Throws<ArgumentException>(() =>
            CartItem.Create(Guid.NewGuid(), title!, 10m));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void Create_NegativeUnitPrice_Throws(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            CartItem.Create(Guid.NewGuid(), "Any", price));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-5)]
    public void Create_NegativeDiscount_Throws(decimal discount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            CartItem.Create(Guid.NewGuid(), "Any", 10m, discount));
    }

    [Fact]
    public void Create_DiscountGreaterThanPrice_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            CartItem.Create(Guid.NewGuid(), "Any", 10m, 11m));
    }
}
