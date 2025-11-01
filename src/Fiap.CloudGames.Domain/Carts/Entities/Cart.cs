namespace Fiap.CloudGames.Domain.Carts.Entities;

/// <summary>Aggregate root do Carrinho.</summary>
public sealed class Cart
{
    public Guid Id { get; private set; }

    /// <summary>Dono do carrinho.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Email do dono (conveniência para exibição/auditoria).</summary>
    public string UserEmail { get; private set; } = string.Empty;

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items;

    /// <summary>Total atual do carrinho (soma de FinalPrice dos itens).</summary>
    public decimal TotalValue { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private Cart() { }

    private Cart(Guid id, Guid userId, string? userEmail)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId obrigatório.", nameof(userId));

        Id = id;
        UserId = userId;
        UserEmail = (userEmail ?? string.Empty).Trim();

        TotalValue = 0m;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    /// <summary>Fábrica de criação do carrinho.</summary>
    public static Cart Create(Guid userId, string? userEmail)
        => new Cart(Guid.NewGuid(), userId, userEmail);

    /// <summary>Retorna true se já existe item para o Game informado (regra 1 licença por jogo).</summary>
    public bool Contains(Guid gameId) => _items.Any(i => i.GameId == gameId);

    /// <summary>Adiciona um jogo ao carrinho (snapshot de título e preço atual).</summary>
    public void AddItem(Guid gameId, string title, decimal unitPrice, decimal discount = 0m)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("GameId inválido.", nameof(gameId));
        title = (title ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título do jogo é obrigatório.", nameof(title));
        if (unitPrice < 0m) throw new ArgumentOutOfRangeException(nameof(unitPrice), "Preço não pode ser negativo.");
        if (discount < 0m) throw new ArgumentOutOfRangeException(nameof(discount), "Desconto não pode ser negativo.");
        if (discount > unitPrice) throw new ArgumentException("Desconto não pode exceder o preço.", nameof(discount));

        // 1 licença por jogo: se já existe, não duplica.
        if (Contains(gameId)) return;

        var item = CartItem.Create(gameId, title, unitPrice, discount);
        _items.Add(item);

        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Remove um jogo do carrinho (ignora se não existir).</summary>
    public void RemoveItem(Guid gameId)
    {
        var it = _items.FirstOrDefault(i => i.GameId == gameId);
        if (it is null) return;

        _items.Remove(it);
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Remove todos os itens (no-op se já vazio).</summary>
    public void Clear()
    {
        if (_items.Count == 0) return;
        _items.Clear();
        RecalculateTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateTotal()
        => TotalValue = _items.Sum(i => i.FinalPrice);
}