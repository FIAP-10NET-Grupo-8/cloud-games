namespace Fiap.CloudGames.Domain.Carts.Entities;

/// <summary>Item do carrinho (snapshot por jogo).</summary>
public sealed class CartItem
{
    public Guid Id { get; private set; }

    /// <summary>Identificador do jogo (referência ao catálogo em outro bounded context).</summary>
    public Guid GameId { get; private set; }

    /// <summary>Título do jogo no momento da adição (snapshot).</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Preço unitário capturado na adição.</summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>Desconto aplicado ao item (opcional, padrão 0).</summary>
    public decimal Discount { get; private set; }

    /// <summary>Preço final do item (UnitPrice - Discount).</summary>
    public decimal FinalPrice => UnitPrice - Discount;

    private CartItem() { }

    private CartItem(Guid id, Guid gameId, string title, decimal unitPrice, decimal discount)
    {
        if (gameId == Guid.Empty) throw new ArgumentException("GameId inválido.", nameof(gameId));
        title = (title ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título obrigatório.", nameof(title));
        if (unitPrice < 0m) throw new ArgumentOutOfRangeException(nameof(unitPrice), "Preço não pode ser negativo.");
        if (discount < 0m) throw new ArgumentOutOfRangeException(nameof(discount), "Desconto não pode ser negativo.");
        if (discount > unitPrice) throw new ArgumentException("Desconto não pode exceder o preço.", nameof(discount));

        Id = id;
        GameId = gameId;
        Title = title;
        UnitPrice = unitPrice;
        Discount = discount;
    }

    public static CartItem Create(Guid gameId, string title, decimal unitPrice, decimal discount = 0m)
        => new CartItem(Guid.NewGuid(), gameId, title, unitPrice, discount);
}