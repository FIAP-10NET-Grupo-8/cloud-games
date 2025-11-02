using Fiap.CloudGames.Domain.Carts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> b)
    {
        b.ToTable("CartItems");
        b.HasKey(i => i.Id);

        b.Property(i => i.GameId)
            .IsRequired();

        b.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(200);

        b.Property(i => i.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        b.Property(i => i.Discount)
            .IsRequired()
            .HasPrecision(18, 2);

        b.HasIndex("CartId", nameof(CartItem.GameId))
            .IsUnique();
    }
}
