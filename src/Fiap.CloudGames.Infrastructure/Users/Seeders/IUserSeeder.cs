using System.Threading.Tasks;

namespace Fiap.CloudGames.Infrastructure.Users.Seeders;

public interface IUserSeeder
{
    Task SeedAsync(CancellationToken ct);
}
