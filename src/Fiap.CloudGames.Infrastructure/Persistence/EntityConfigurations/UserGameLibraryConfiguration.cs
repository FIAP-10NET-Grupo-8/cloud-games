using Fiap.CloudGames.Domain.UserGamesLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.CloudGames.Infrastructure.Persistence.EntityConfigurations
{
    public class UserGameLibraryConfiguration : IEntityTypeConfiguration<UserGameLibrary>
    {
        public void Configure(EntityTypeBuilder<UserGameLibrary> builder)
        {
            builder.ToTable("Library");

            builder.HasKey(ug => new { ug.UserId, ug.GameId });

            builder.HasOne(ug => ug.User)
                   .WithMany(u => u.UserGamesLibrary)
                   .HasForeignKey(ug => ug.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ug => ug.Game)
                   .WithMany(g => g.UserGamesLibrary)
                   .HasForeignKey(ug => ug.GameId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ug => ug.PurchaseDate)
                   .IsRequired();
        }
    }
}
