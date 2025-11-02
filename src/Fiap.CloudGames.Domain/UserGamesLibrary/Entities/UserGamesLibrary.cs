using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Users.Entities;

namespace Fiap.CloudGames.Domain.UserGamesLibrary.Entities;

/// <summary>
/// Entidade de ligação para a relação M:N entre User e Game,
/// representando um item possuído na "biblioteca" do usuário.
/// </summary>
public class UserGameLibrary
{
    public Guid UserId { get; private set; }
    public Guid GameId { get; private set; }

    /// <summary>Momento (UTC) em que o jogo foi adquirido/adicionado.</summary>
    public DateTime PurchaseDate { get; private set; }

    public User? User { get; private set; } = null!;
    public Game? Game { get; private set; } = null!;

    private UserGameLibrary() { }

    private UserGameLibrary(Guid userId, Guid gameId, DateTime purchaseDateUtc)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId inválido.", nameof(userId));
        if (gameId == Guid.Empty) throw new ArgumentException("GameId inválido.", nameof(gameId));
        if (purchaseDateUtc.Kind != DateTimeKind.Utc)
            purchaseDateUtc = DateTime.SpecifyKind(purchaseDateUtc, DateTimeKind.Utc);
        if (purchaseDateUtc > DateTime.UtcNow)
            throw new ArgumentOutOfRangeException(nameof(purchaseDateUtc), "Data de compra não pode ser futura.");

        UserId = userId;
        GameId = gameId;
        PurchaseDate = purchaseDateUtc;
    }

    /// <summary>Factory com validações básicas.</summary>
    public static UserGameLibrary Create(Guid userId, Guid gameId, DateTime purchaseDateUtc)
        => new(userId, gameId, purchaseDateUtc);
}
