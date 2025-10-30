using Fiap.CloudGames.Domain.Orders.Entities;
using Fiap.CloudGames.Domain.Orders.Enums;

namespace Fiap.CloudGames.Tests.Orders;

public class OrderTests
{
    [Fact]
    public void Create_ValidOrder_SetsSnapshotAndTotals()
    {
        var userId = Guid.NewGuid();
        var items = new[]
        {
            OrderItem.Create(Guid.NewGuid(), "Game A", 2, 15m),
            OrderItem.Create(Guid.NewGuid(), "Game B", 1, 20m)
        };

        var order = Order.Create(
            userId: userId,
            customerEmail: "user@example.com",
            items: items,
            idempotencyKey: "cart-123:v1",
            paymentTransactionId: null
        );

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(userId, order.UserId);
        Assert.Equal("user@example.com", order.CustomerEmail);
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(50m, order.TotalValue); // 2*15 + 1*20
        Assert.Equal(OrderStatus.PendingPayment, order.Status);
        Assert.False(order.RefundRequested);
        Assert.NotEqual(default, order.CreatedAt);
        Assert.Null(order.UpdatedAt);
    }

    [Fact]
    public void Create_EmptyItems_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            Order.Create(Guid.NewGuid(), "x@y.com", Array.Empty<OrderItem>()));
    }

    [Fact]
    public void Create_InvalidUserId_Throws()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        Assert.Throws<ArgumentException>(() =>
            Order.Create(Guid.Empty, "x@y.com", items));
    }

    [Fact]
    public void MarkPaid_TransitionsToPaid_AndSetsTransactionId()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);

        order.MarkPaid("txn-001");

        Assert.Equal(OrderStatus.Paid, order.Status);
        Assert.Equal("txn-001", order.PaymentTransactionId);
        Assert.NotNull(order.UpdatedAt);
    }

    [Fact]
    public void Cancel_FromPending_SetsCancelled()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);

        order.Cancel("user request");

        Assert.Equal(OrderStatus.Cancelled, order.Status);
        Assert.NotNull(order.UpdatedAt);
    }

    [Fact]
    public void Cancel_FromPaid_Throws()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);
        order.MarkPaid("txn-1");

        Assert.Throws<InvalidOperationException>(() => order.Cancel("n/a"));
    }

    [Fact]
    public void RequestRefund_FromPending_SetsRefundRequestedAndReason()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);

        var t0 = DateTime.UtcNow;
        order.RequestRefund("arrependimento", t0);

        Assert.True(order.RefundRequested);
        Assert.Equal(OrderStatus.RefundRequested, order.Status);
        Assert.Equal("arrependimento", order.RefundReason);
        Assert.Equal(t0, order.RefundRequestDate);
        Assert.NotNull(order.UpdatedAt);
    }

    [Fact]
    public void RequestRefund_Twice_Throws()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);
        order.RequestRefund("motivo", DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() =>
            order.RequestRefund("outro", DateTime.UtcNow));
    }

    [Fact]
    public void MarkRefunded_WithoutRequest_Throws()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);

        Assert.Throws<InvalidOperationException>(() => order.MarkRefunded(DateTime.UtcNow));
    }

    [Fact]
    public void MarkRefunded_AfterRequest_TransitionsToRefunded()
    {
        var items = new[] { OrderItem.Create(Guid.NewGuid(), "Game", 1, 10m) };
        var order = Order.Create(Guid.NewGuid(), "u@e.com", items);
        order.RequestRefund("motivo", DateTime.UtcNow);

        var t0 = DateTime.UtcNow;
        order.MarkRefunded(t0);

        Assert.Equal(OrderStatus.Refunded, order.Status);
        Assert.Equal(t0, order.RefundDate);
        Assert.NotNull(order.UpdatedAt);
    }
}
