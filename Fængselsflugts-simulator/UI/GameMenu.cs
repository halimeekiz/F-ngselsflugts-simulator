using Fængselsflugts_simulator.Models.Prisoners;

namespace Fængselsflugts_simulator.UI
{
	/// <summary>
	/// Spillets hovedmenu styres med piletaster og Enter.
	/// </summary>
	internal static class GameMenu
	{
		/// <summary>
		/// Viser hovedmenuen og returnerer det valgte menupunkt.
		/// </summary>
		public static int Show(Prisoner player)
		{
			string[] options =
			{
				"Start flugten",
				"Se fængselskort",
				"Se dine evner",
				"Se hændelser",
				"Se tildelingsstrategier",
				"Afslut"
			};

			int selected = 0;

			while (true)
			{
				Console.Clear();

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("╔════════════════════════════════════════╗");
				Console.WriteLine("║      FÆNGSELSFLUGTS-SIMULATOR          ║");
				Console.WriteLine("╚════════════════════════════════════════╝");
				Console.ResetColor();

				Console.WriteLine();

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine("SPILLER");
				Console.ResetColor();

				Console.WriteLine($"Fange : {player.Name}");
				Console.WriteLine($"Power : {player.PowerLevel}");

				Console.WriteLine();
				Console.WriteLine("────────────────────────────────────────");
				Console.WriteLine();

				for (int i = 0; i < options.Length; i++)
				{
					if (i == selected)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write($"▶ ");

						if (options[i] == "Start flugten")
							Console.ForegroundColor = ConsoleColor.Green;

						if (options[i] == "Afslut")
							Console.ForegroundColor = ConsoleColor.Red;

						Console.WriteLine(options[i]);
						Console.ResetColor();
					}
					else
					{
						Console.WriteLine($"  {options[i]}");
					}
				}

				Console.WriteLine();
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("↑ ↓ Vælg    ENTER Bekræft");
				Console.ResetColor();

				ConsoleKey key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.UpArrow)
					selected = (selected - 1 + options.Length) % options.Length;

				if (key == ConsoleKey.DownArrow)
					selected = (selected + 1) % options.Length;

				if (key == ConsoleKey.Enter)
					return selected;
			}
		}
	}
}