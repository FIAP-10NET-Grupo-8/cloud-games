namespace Fiap.CloudGames.Domain.Carts.Entities;
public sealed class CartItem
{
    public Guid Id { get; private set; }
    public Guid GameId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal FinalPrice => UnitPrice - Discount;

    private CartItem() { }

    public static CartItem Create(Guid gameId, string title, decimal unitPrice, decimal discount = 0m)
        => new CartItem();
}