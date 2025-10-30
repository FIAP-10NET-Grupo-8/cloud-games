using Fiap.CloudGames.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> b)
    {
        b.ToTable("OrderItems");
        b.HasKey(i => i.Id);

        b.Property(i => i.GameId).IsRequired();

        b.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(255);

        b.Property(i => i.Quantity)
            .IsRequired();

        b.Property(i => i.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        b.Property(i => i.LineTotal)
            .IsRequired()
            .HasPrecision(18, 2);

        b.HasIndex(i => i.GameId);
    }
}
