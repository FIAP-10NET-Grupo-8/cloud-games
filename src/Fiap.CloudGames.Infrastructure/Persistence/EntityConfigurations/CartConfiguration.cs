using Fiap.CloudGames.Domain.Carts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> b)
    {
        b.ToTable("Carts");
        b.HasKey(c => c.Id);

        b.Property(c => c.UserId).IsRequired();

        b.Property(c => c.UserEmail)
            .HasMaxLength(255);

        b.Property(c => c.TotalValue)
            .IsRequired()
            .HasPrecision(18, 2);

        b.Property(c => c.CreatedAt).IsRequired();
        b.Property(c => c.UpdatedAt);

        b.HasIndex(c => c.UserId).IsUnique();

        b.HasMany(c => c.Items)
            .WithOne()
            .HasForeignKey("CartId")
            .OnDelete(DeleteBehavior.Cascade);

        b.Navigation(c => c.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
