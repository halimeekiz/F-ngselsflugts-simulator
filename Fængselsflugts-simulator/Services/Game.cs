using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.UI;
using Fængselsflugts_simulator.Enums;
using Fængselsflugts_simulator.Strategies;

namespace Fængselsflugts_simulator.Services
{
	/// <summary>
	/// Styrer spillets overordnede flow: intro, fangevalg, menu og kontrolcentral.
	/// </summary>
	internal class Game
	{
		/// <summary>
		/// Starter spillet og viser hovedmenuen, indtil spilleren afslutter.
		/// </summary>
		public void Start()
		{
			GameIntro.Show();

			Prisoner player = PrisonerSelection.ChoosePrisoner();
			PrisonLocation currentLocation = PrisonLocation.CellBlockA;

			// Dependency Inversion: strategien gives udefra via constructor injection.
			PrisonControlCenter controlCenter = new PrisonControlCenter(
				new FirstAvailablePrisonerStrategy());

			RegisterPrisoners(controlCenter, player);

			bool running = true;

			while (running)
			{
				int choice = GameMenu.Show(player);

				switch (choice)
				{
					case 0:
						EscapeGame escapeGame = new EscapeGame(controlCenter);
						escapeGame.Start(player);
						break;

					case 1:
						ShowMap(currentLocation);
						break;

					case 2:
						ShowAbilities(player);
						break;

					case 3:
						ShowControlCenter(controlCenter);
						break;

					case 4:
						running = false;
						break;
				}
			}
		}

		/// <summary>
		/// Registrerer den valgte spiller og de øvrige fangetyper i kontrolcentralen.
		/// </summary>
		private static void RegisterPrisoners(
			PrisonControlCenter controlCenter,
			Prisoner player)
		{
			controlCenter.RegisterPrisoner(player);

			Prisoner[] otherPrisoners =
			{
				new EscapeArtist(10, "Luna"),
				new HackerPrisoner(11, "Omar"),
				new StrongPrisoner(12, "Bo")
			};

			foreach (Prisoner other in otherPrisoners)
			{
				if (other.GetType() == player.GetType())
				{
					continue;
				}

				other.IsAvailable = false;
				other.Status = PrisonerStatus.InCell;
				controlCenter.RegisterPrisoner(other);
			}
		}

		private void ShowMap(PrisonLocation currentLocation)
		{
			Console.Clear();
			PrisonMap.Draw(currentLocation);
			WaitForEscape();
		}

		private void ShowAbilities(Prisoner player)
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("DINE EVNER\n");
			Console.ResetColor();

			Console.WriteLine($"Fange: {player.Name}");
			Console.WriteLine($"Power: {player.PowerLevel}\n");

			// Interfaces afgør hvilke evner den valgte fange har.
			if (player is ILockPicker)
				Console.WriteLine("🔓 Åbn eller tving låste døre op");

			if (player is IHacker)
				Console.WriteLine("💻 Hack sikkerhedssystemer");

			if (player is ISneaky)
				Console.WriteLine("👣 Snig dig forbi vagter");

			if (player is ISecurityHacker)
			{
				Console.WriteLine("📹 Deaktiver kameraer");
				Console.WriteLine("🚨 Deaktiver alarm");
			}

			if (player is ISuperStrong)
				Console.WriteLine("💥 Bryd døre med rå styrke");

			if (player is IObstacleMover)
				Console.WriteLine("💪 Flyt tunge forhindringer");

			WaitForEscape();
		}

		/// <summary>
		/// Viser kontrolcentralens collections af hændelser og ledige fanger.
		/// </summary>
		private void ShowControlCenter(PrisonControlCenter controlCenter)
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("KONTROLCENTRALEN\n");
			Console.ResetColor();

			Console.WriteLine("HÆNDELSER:");
			controlCenter.ShowIncidents();

			Console.WriteLine("\nUAFKLAREDE HÆNDELSER:");
			var unresolved = controlCenter.GetUnresolvedIncidents();
			if (unresolved.Count == 0)
			{
				Console.WriteLine("Ingen.");
			}
			else
			{
				foreach (var incident in unresolved)
				{
					Console.WriteLine(
						$"- {incident.Description} ({incident.Location}) [{incident.Severity}]");
				}
			}

			Console.WriteLine("\nLEDIGE FANGER:");
			var available = controlCenter.GetAvailablePrisoners();
			if (available.Count == 0)
			{
				Console.WriteLine("Ingen ledige fanger.");
			}
			else
			{
				foreach (var prisoner in available)
				{
					Console.WriteLine($"- {prisoner.Name}");
				}
			}

			WaitForEscape();
		}

		private void WaitForEscape()
		{
			Console.WriteLine("\nTryk ESC for at gå tilbage");

			while (Console.ReadKey(true).Key != ConsoleKey.Escape)
			{
			}
		}
	}
}