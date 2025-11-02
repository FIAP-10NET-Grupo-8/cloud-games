using Fiap.CloudGames.Domain.Promotions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations;

public sealed class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
	public void Configure(EntityTypeBuilder<Promotion> b)
	{
		b.ToTable("Promotions");
		b.HasKey(p => p.Id);

		b.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(200);

		b.OwnsOne(p => p.Period, periodBuilder =>
		{
			periodBuilder.Property(p => p.StartDate)
				.HasColumnName("StartDate")
				.IsRequired();

			periodBuilder.Property(p => p.EndDate)
				.HasColumnName("EndDate")
				.IsRequired();
		});

		b.OwnsOne(p => p.Discount, discountBuilder =>
		{
			discountBuilder.Property(d => d.Percentage)
				.HasColumnName("DiscountPercentage")
				.HasPrecision(5, 2)
				.IsRequired();
		});

		b.Property(p => p.Status)
			.HasConversion<string>()
			.HasMaxLength(50)
			.IsRequired();

		b.Property(p => p.CreatedAt)
			.IsRequired();

		b.Property(p => p.UpdatedAt)
			.IsRequired(false);

		b.OwnsMany(p => p.EligibleGames, egb =>
		{
			egb.ToTable("PromotionItems");
			egb.WithOwner().HasForeignKey("PromotionId");
			egb.HasKey("PromotionId", "GameId");
			egb.Property(eg => eg.GameId)
				.HasColumnName("GameId")
				.IsRequired();
		});

		b.Metadata
			.FindNavigation(nameof(Promotion.EligibleGames))!
			.SetPropertyAccessMode(PropertyAccessMode.Field);
	}
}
