using Fiap.CloudGames.Domain.Games.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> b)
    {
        b.ToTable("Games");
        b.HasKey(g => g.Id);

        b.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(255);

        b.HasIndex(g => g.Title).IsUnique();

        b.Property(g => g.Description).HasMaxLength(1000);

        b.Property(g => g.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        b.Property(g => g.ReleaseDate).IsRequired();

        b.Property(g => g.Developer)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(g => g.Publisher)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(g => g.Genre).HasMaxLength(100);
        b.Property(g => g.Platforms).HasMaxLength(200);

        b.Property(g => g.Active).HasDefaultValue(true);
        b.Property(g => g.CreatedAt).IsRequired();
        b.Property(g => g.UpdatedAt);
    }
}
