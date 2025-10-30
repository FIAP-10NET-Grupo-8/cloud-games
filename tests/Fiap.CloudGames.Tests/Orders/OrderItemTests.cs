using Fiap.CloudGames.Domain.Orders.Entities;

namespace Fiap.CloudGames.Tests.Orders;

public class OrderItemTests
{
    [Fact]
    public void Create_ValidItem_ComputesLineTotal()
    {
        var item = OrderItem.Create(
            gameId: Guid.NewGuid(),
            title: "C# Game",
            quantity: 3,
            unitPrice: 10m
        );

        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal("C# Game", item.Title);
        Assert.Equal(3, item.Quantity);
        Assert.Equal(10m, item.UnitPrice);
        Assert.Equal(30m, item.LineTotal);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidQuantity_Throws(int qty)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            OrderItem.Create(Guid.NewGuid(), "T", qty, 5m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    public void Create_InvalidUnitPrice_Throws(decimal price)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            OrderItem.Create(Guid.NewGuid(), "T", 1, price));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidTitle_Throws(string title)
    {
        Assert.Throws<ArgumentException>(() =>
            OrderItem.Create(Guid.NewGuid(), title!, 1, 1m));
    }
}
