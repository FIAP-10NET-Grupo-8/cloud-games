using Fiap.CloudGames.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> b)
    {
        b.ToTable("Orders");
        b.HasKey(o => o.Id);

        b.Property(o => o.UserId).IsRequired();

        b.Property(o => o.CustomerEmail)
            .HasMaxLength(255);

        b.Property(o => o.TotalValue)
            .IsRequired()
            .HasPrecision(18, 2);

        b.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        b.Property(o => o.RefundRequested)
            .HasDefaultValue(false);

        b.Property(o => o.RefundReason)
            .HasMaxLength(500);

        b.Property(o => o.RefundRequestDate);
        b.Property(o => o.RefundDate);

        b.Property(o => o.PaymentTransactionId)
            .HasMaxLength(100);

        b.Property(o => o.IdempotencyKey)
            .HasMaxLength(100);

        b.Property(o => o.CreatedAt).IsRequired();
        b.Property(o => o.UpdatedAt);

        b.HasIndex(o => o.UserId);
        b.HasIndex(o => o.CreatedAt);
        b.HasIndex(o => new { o.UserId, o.IdempotencyKey });

        b.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
