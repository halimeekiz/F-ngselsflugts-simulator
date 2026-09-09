using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.UI;

namespace Fængselsflugts_simulator.Services
{
	// Styrer spillets overordnede flow.
	internal class Game
	{
		public void Start()
		{
			GameIntro.Show();

			Prisoner player = PrisonerSelection.ChoosePrisoner();

			bool running = true;

			while (running)
			{
				int choice = GameMenu.Show(player);

				switch (choice)
				{
					case 0:
						// Selve flugten bygges herfra senere
						break;

					case 1:
						ShowMap();
						break;

					case 2:
						ShowAbilities(player);
						break;

					case 3:
						running = false;
						break;
				}
			}
		}

		private void ShowMap()
		{
			Console.Clear();
			PrisonMap.Draw();
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

			if (player is EscapeArtist)
			{
				Console.WriteLine("🔓 Dirk låse");
				Console.WriteLine("💻 Simpel hacking");
				Console.WriteLine("👣 Snigeevne");
			}
			else if (player is HackerPrisoner)
			{
				Console.WriteLine("💻 Hack sikkerhed");
				Console.WriteLine("📹 Deaktiver kameraer");
				Console.WriteLine("🚨 Manipuler alarm");
			}
			else if (player is StrongPrisoner)
			{
				Console.WriteLine("💥 Bryd døre");
				Console.WriteLine("🔓 Tving låse op");
				Console.WriteLine("💪 Flyt forhindringer");
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