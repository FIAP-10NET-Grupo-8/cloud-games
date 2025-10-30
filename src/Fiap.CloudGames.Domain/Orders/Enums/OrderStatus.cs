namespace Fiap.CloudGames.Domain.Orders.Enums;

public enum OrderStatus
{
    PendingPayment = 0,
    Paid = 1,
    Cancelled = 2,
    RefundRequested = 3,
    Refunded = 4
}