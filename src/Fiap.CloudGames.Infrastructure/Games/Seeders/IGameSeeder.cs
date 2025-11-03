using System;

namespace Fiap.CloudGames.Infrastructure.Games.Seeders;

public interface IGameSeeder
{
	Task SeedAsync(CancellationToken ct);
}
