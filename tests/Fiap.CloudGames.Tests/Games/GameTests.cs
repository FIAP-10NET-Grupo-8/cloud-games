using Fiap.CloudGames.Domain.Games.Entities;

namespace Fiap.CloudGames.Tests.Games;

public class GameTests
{
    [Fact]
    public void Create_ValidGame_SetsProperties()
    {
        var release = new DateTime(2026, 1, 1);
        var game = Game.Create(
            title: "Intro to C#",
            description: "Educational game to learn C# basics",
            price: 0m,
            releaseDate: release,
            developer: "FIAP",
            publisher: "FIAP",
            genre: "Educational",
            platforms: "Cloud-Hosted"
        );

        Assert.NotEqual(Guid.Empty, game.Id);
        Assert.Equal("Intro to C#", game.Title);
        Assert.Equal("Educational game to learn C# basics", game.Description);
        Assert.Equal(0m, game.Price);
        Assert.Equal(release, game.ReleaseDate);
        Assert.Equal("FIAP", game.Developer);
        Assert.Equal("FIAP", game.Publisher);
        Assert.Equal("Educational", game.Genre);
        Assert.Equal("Cloud-Hosted", game.Platforms);
        Assert.True(game.Active);
        Assert.NotEqual(default(DateTime), game.CreatedAt);
        Assert.Null(game.UpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidTitle_Throws(string title)
    {
        Assert.Throws<ArgumentException>(() => Game.Create(
            title,
            description: "d",
            price: 0m,
            releaseDate: DateTime.UtcNow,
            developer: "Dev",
            publisher: "Pub",
            genre: null,
            platforms: null
        ));
    }

    [Fact]
    public void Create_NegativePrice_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Game.Create(
            "Title",
            description: "d",
            price: -1m,
            releaseDate: DateTime.UtcNow,
            developer: "Dev",
            publisher: "Pub",
            genre: null,
            platforms: null
        ));
    }

    [Fact]
    public void Update_Valid_UpdatesPropertiesAndSetsUpdatedAt()
    {
        var game = Game.Create("T", "D", 0m, DateTime.UtcNow, "Dev", "Pub", null, null);
        var originalCreated = game.CreatedAt;

        Thread.Sleep(10);

        game.Update(
            title: "New Title",
            description: "New Desc",
            price: 0m,
            releaseDate: new DateTime(2026, 1, 1),
            developer: "New Dev",
            publisher: "New Pub",
            genre: "Educational",
            platforms: "Cloud",
            active: false
        );

        Assert.Equal("New Title", game.Title);
        Assert.Equal("New Desc", game.Description);
        Assert.Equal(0m, game.Price);
        Assert.Equal(new DateTime(2026, 1, 1), game.ReleaseDate);
        Assert.Equal("New Dev", game.Developer);
        Assert.Equal("New Pub", game.Publisher);
        Assert.Equal("Educational", game.Genre);
        Assert.Equal("Cloud", game.Platforms);
        Assert.False(game.Active);
        Assert.NotNull(game.UpdatedAt);
        Assert.True(game.UpdatedAt > originalCreated);
    }

    [Fact]
    public void Update_InvalidTitle_Throws()
    {
        var game = Game.Create("T", "D", 0m, DateTime.UtcNow, "Dev", "Pub", null, null);
        Assert.Throws<ArgumentException>(() => game.Update(
            title: "   ",
            description: "d",
            price: 1m,
            releaseDate: DateTime.UtcNow,
            developer: "Dev",
            publisher: "Pub",
            genre: null,
            platforms: null,
            active: true
        ));
    }

    [Fact]
    public void Update_NegativePrice_Throws()
    {
        var game = Game.Create("T", "D", 0m, DateTime.UtcNow, "Dev", "Pub", null, null);
        Assert.Throws<ArgumentOutOfRangeException>(() => game.Update(
            title: "Title",
            description: "d",
            price: -5m,
            releaseDate: DateTime.UtcNow,
            developer: "Dev",
            publisher: "Pub",
            genre: null,
            platforms: null,
            active: true
        ));
    }

    [Fact]
    public void Deactivate_SetsActiveFalseAndUpdatesUpdatedAt()
    {
        var game = Game.Create("T", "D", 0m, DateTime.UtcNow, "Dev", "Pub", null, null);
        Assert.Null(game.UpdatedAt);

        var t0 = DateTime.UtcNow;
        game.Deactivate();
        var t1 = DateTime.UtcNow;

        Assert.False(game.Active);
        Assert.NotNull(game.UpdatedAt);
        Assert.InRange(game.UpdatedAt!.Value, t0, t1);
    }

    [Fact]
    public void Activate_SetsActiveTrueAndUpdatesUpdatedAt()
    {
        var game = Game.Create("T", "D", 0m, DateTime.UtcNow, "Dev", "Pub", null, null);
        game.Deactivate();
        var afterDeactivate = game.UpdatedAt!.Value;

        var t0 = DateTime.UtcNow;
        game.Activate();
        var t1 = DateTime.UtcNow;

        Assert.True(game.Active);
        Assert.NotNull(game.UpdatedAt);
        Assert.True(game.UpdatedAt!.Value > afterDeactivate);
        Assert.InRange(game.UpdatedAt!.Value, t0, t1);
    }
}