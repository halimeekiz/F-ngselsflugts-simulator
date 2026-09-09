using Fængselsflugts_simulator.Interfaces;
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

		private void WaitForEscape()
		{
			Console.WriteLine("\nTryk ESC for at gå tilbage");

			while (Console.ReadKey(true).Key != ConsoleKey.Escape)
			{
			}
		}
	}
}