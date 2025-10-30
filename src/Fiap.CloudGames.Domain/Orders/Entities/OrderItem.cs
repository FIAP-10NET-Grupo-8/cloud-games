namespace Fiap.CloudGames.Domain.Orders.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid GameId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal { get; private set; }

    private OrderItem() { }

    private OrderItem(Guid id, Guid gameId, string title, int quantity, decimal unitPrice)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("GameId obrigatório.", nameof(gameId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título obrigatório.", nameof(title));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantidade deve ser > 0.");
        if (unitPrice <= 0) throw new ArgumentOutOfRangeException(nameof(unitPrice), "Preço unitário deve ser > 0.");

        Id = id;
        GameId = gameId;
        Title = title.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = unitPrice * quantity;
    }

    public static OrderItem Create(Guid gameId, string title, int quantity, decimal unitPrice)
        => new(Guid.NewGuid(), gameId, title, quantity, unitPrice);
}