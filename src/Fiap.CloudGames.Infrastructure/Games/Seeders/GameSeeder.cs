using Fiap.CloudGames.Domain.Games.Entities;
using Fiap.CloudGames.Domain.Games.Repositories;

namespace Fiap.CloudGames.Infrastructure.Games.Seeders;

public class GameSeeder(IGameRepository gameRepository) : IGameSeeder
{
	private readonly IGameRepository _gameRepository = gameRepository;

	public async Task SeedAsync(CancellationToken ct)
	{
		List<Game> gamesToSeed = [
			Game.Create(
				"Elden Ring",
				"O aclamado RPG de ação em mundo aberto da FromSoftware, vencedor de Jogo do Ano, onde os jogadores exploram as Terras Intermédias.",
				249.9m,
				new DateTime(2022, 02, 25),
				"FromSoftware Inc.",
				"Bandai Namco Entertainment",
				"Action, RPG, Open World",
				"PC, PlayStation 5, Xbox Series X/S, PS4, Xbox One"
			),
			Game.Create(
				"Baldur's Gate 3",
				"Um RPG baseado em Dungeons & Dragons da Larian Studios, vencedor de Jogo do Ano, focado em narrativa e liberdade do jogador.",
				199.99m,
				new DateTime(2023, 08, 03),
				"Larian Studios",
				"Larian Studios",
				"RPG, Strategy, Adventure",
				"PC, PlayStation 5, Xbox Series X/S, macOS"
			),
			Game.Create(
				"Assassin's Creed Shadows",
				"Um RPG de ação em mundo aberto ambientado no Japão feudal, seguindo as histórias interligadas de uma shinobi e um samurai.",
				349.0m,
				new DateTime(2025, 03, 20),
				"Ubisoft Quebec",
				"Ubisoft",
				"Action, RPG, Open World",
				"PC, PlayStation 5, Xbox Series X/S, macOS"
			),
			Game.Create(
				"Star Wars Outlaws",
				"O primeiro jogo de mundo aberto de Star Wars, focado na jornada de uma fora-da-lei em ascensão no submundo da galáxia.",
				349.99m,
				new DateTime(2024, 08, 30),
				"Massive Entertainment (Ubisoft)",
				"Ubisoft",
				"Action-Adventure, Open World",
				"PC, PlayStation 5, Xbox Series X/S"
			),
			Game.Create(
				"Helldivers 2",
				"Um shooter cooperativo em terceira pessoa focado em esquadrões, onde os jogadores lutam contra ameaças alienígenas para proteger a Super Terra.",
				199.5m,
				new DateTime(2024, 02, 08),
				"Arrowhead Game Studios",
				"PlayStation PC LLC",
				"Third-Person Shooter, Co-op",
				"PC, PlayStation 5"
			),
			Game.Create(
				"Hades II",
				"Sequência do aclamado roguelike, onde a Princesa do Submundo usa magia sombria para enfrentar o Titã do Tempo.",
				98.99m,
				new DateTime(2024, 05, 06),
				"Supergiant Games",
				"Supergiant Games",
				"Action, RPG, Roguelike",
				"PC, macOS"
			),
			Game.Create(
				"Cyberpunk 2077: Ultimate Edition",
				"A versão completa do RPG de ação em mundo aberto, ambientado em Night City. Inclui a expansão Phantom Liberty e todas as atualizações.",
				249.9m,
				new DateTime(2023, 12, 05),
				"CD PROJEKT RED",
				"CD PROJEKT RED",
				"RPG, Action, Open World",
				"PC, PlayStation 5, Xbox Series X/S"
			),
			Game.Create(
				"Palworld",
				"Um jogo de sobrevivência em mundo aberto com criação e captura de monstros, onde os 'Pals' podem ser usados em combate ou para trabalho.",
				88.99m,
				new DateTime(2024, 01, 19),
				"Pocketpair, Inc.",
				"Pocketpair, Inc.",
				"Survival, Open World, Crafting",
				"PC, Xbox One, Xbox Series X/S"
			),
			Game.Create(
				"S.T.A.L.K.E.R. 2: Heart of Chornobyl",
				"Um FPS de sobrevivência e terror ambientado na Zona de Exclusão de Chornobyl, com um mundo aberto e narrativa não linear.",
				249.0m,
				new DateTime(2024, 11, 20),
				"GSC Game World",
				"GSC Game World",
				"Action, Adventure, RPG, FPS",
				"PC, Xbox Series X/S"
			),
			Game.Create(
				"Dragon Age: The Veilguard",
				"Sequência do RPG de ação da BioWare, onde o jogador deve recrutar uma equipe de companheiros para lutar contra deuses antigos.",
				249.0m,
				new DateTime(2024, 10, 30),
				"BioWare",
				"Electronic Arts",
				"Action, RPG",
				"PC, PlayStation 5, Xbox Series X/S"
			),
			Game.Create(
				"Hades",
				"Um roguelike de ação premiado onde o jogador, na pele do príncipe Zagreu, desafia o deus da morte para escapar do submundo grego.",
				84.99m,
				new DateTime(2020, 09, 17),
				"Supergiant Games",
				"Supergiant Games",
				"Action, RPG, Roguelike",
				"PC, PlayStation 5, Xbox Series X/S, PS4, Xbox One, macOS, Nintendo Switch"
			)
		];

		var existingGames = await _gameRepository.GetAllAsync(ct);
		foreach (var game in gamesToSeed)
		{
			if (!existingGames.Any(g => g.Title == game.Title))
			{
				await _gameRepository.AddAsync(game, ct);
			}
		}
	}
}
