using Fiap.CloudGames.Domain.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
        builder.ToTable("Orders");

        builder.HasKey(g => g.Id);

		builder.Property(o => o.PlayerId)
			.IsRequired();

		builder.Property(o => o.PurchaseDate)
			.IsRequired();

		builder.Property(o => o.TotalValue)
			.IsRequired()
			.HasPrecision(18, 2);

		builder.Property(o => o.Status)
			.HasConversion<string>()
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(o => o.RefundRequested)
			.IsRequired();

		builder.Property(o => o.RefundReason)
			.HasMaxLength(1000)
			.IsRequired(false);

		builder.Property(o => o.RefundRequestDate)
			.IsRequired(false);

		builder.Property(o => o.RefundDate)
			.IsRequired(false);

		builder.Property(o => o.PaymentTransactionId)
			.HasMaxLength(255)
			.IsRequired(false);

		builder.Property(o => o.PaymentConfirmed)
			.IsRequired();

		builder.Property(o => o.CreatedAt)
			.IsRequired();

		builder.Property(o => o.UpdatedAt)
			.IsRequired(false);
	}
}
