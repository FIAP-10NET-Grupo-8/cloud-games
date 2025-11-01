namespace Fiap.CloudGames.Domain.Carts.Entities;
public sealed class Cart
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string UserEmail { get; private set; } = string.Empty;
    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items;
    public decimal TotalValue { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private Cart() { }

    public static Cart Create(Guid userId, string? userEmail) => new Cart();
    public bool Contains(Guid gameId) => false;
    public void AddItem(Guid gameId, string title, decimal unitPrice, decimal discount = 0m) { }
    public void RemoveItem(Guid gameId) { }
    public void Clear() { }
}