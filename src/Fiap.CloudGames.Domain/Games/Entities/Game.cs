namespace Fiap.CloudGames.Domain.Games.Entities;

/// <summary>
/// Entity representing a Game in the catalog.
/// </summary>
public class Game
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public string Developer { get; private set; } = string.Empty;
    public string Publisher { get; private set; } = string.Empty;
    public string? Genre { get; private set; }
    public string? Platforms { get; private set; }
    public bool Active { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    private Game() { }

    private Game(
        Guid id,
        string title,
        string? description,
        decimal price,
        DateTime releaseDate,
        string developer,
        string publisher,
        string? genre,
        string? platforms,
        bool active,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Price = price;
        ReleaseDate = releaseDate;
        Developer = developer;
        Publisher = publisher;
        Genre = genre;
        Platforms = platforms;
        Active = active;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Factory Method to create a new Game with validation and default values.
    /// </summary>
    /// <param name="title">Game title.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="price">Current price (must be &gt;= 0).</param>
    /// <param name="releaseDate">Release date.</param>
    /// <param name="developer">Developer/studio.</param>
    /// <param name="publisher">Publisher.</param>
    /// <param name="genre">Optional genre.</param>
    /// <param name="platforms">Optional free-text platforms.</param>
    /// <returns>New <see cref="Game"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when required strings are empty/whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="price"/> is negative.</exception>
    public static Game Create(
        string title,
        string? description,
        decimal price,
        DateTime releaseDate,
        string developer,
        string publisher,
        string? genre,
        string? platforms)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título obrigatório.", nameof(title));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Preço não pode ser negativo.");
        if (string.IsNullOrWhiteSpace(developer))
            throw new ArgumentException("Desenvolvedor obrigatório.", nameof(developer));
        if (string.IsNullOrWhiteSpace(publisher))
            throw new ArgumentException("Publicadora obrigatória.", nameof(publisher));

        return new Game(
            id: Guid.NewGuid(),
            title: title.Trim(),
            description: description?.Trim(),
            price: price,
            releaseDate: releaseDate,
            developer: developer.Trim(),
            publisher: publisher.Trim(),
            genre: genre?.Trim(),
            platforms: platforms?.Trim(),
            active: true,
            createdAt: DateTime.UtcNow,
            updatedAt: null
        );
    }

    /// <summary>
    /// Updates the game fields enforcing basic invariants.
    /// </summary>
    /// <param name="title">Game title.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="price">Current price (must be &gt;= 0).</param>
    /// <param name="releaseDate">Release date.</param>
    /// <param name="developer">Developer/studio.</param>
    /// <param name="publisher">Publisher.</param>
    /// <param name="genre">Optional genre.</param>
    /// <param name="platforms">Optional free-text platforms.</param>
    /// <param name="active">Whether the game is active.</param>
    /// <exception cref="ArgumentException">Thrown when required strings are empty/whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="price"/> is negative.</exception>
    public void Update(
        string title,
        string? description,
        decimal price,
        DateTime releaseDate,
        string developer,
        string publisher,
        string? genre,
        string? platforms,
        bool active)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título obrigatório.", nameof(title));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Preço não pode ser negativo.");
        if (string.IsNullOrWhiteSpace(developer))
            throw new ArgumentException("Desenvolvedor obrigatório.", nameof(developer));
        if (string.IsNullOrWhiteSpace(publisher))
            throw new ArgumentException("Publicadora obrigatória.", nameof(publisher));

        Title = title.Trim();
        Description = description?.Trim();
        Price = price;
        ReleaseDate = releaseDate;
        Developer = developer.Trim();
        Publisher = publisher.Trim();
        Genre = genre?.Trim();
        Platforms = platforms?.Trim();
        Active = active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Marks the game as active.</summary>
    public void Activate()
    {
        Active = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Marks the game as inactive.</summary>
    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }
}