using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.UI
{
	/// <summary>
	/// Genbrugelig navigationsmenu med piletaster og Enter.
	/// </summary>
	internal static class DirectionMenu
	{
		/// <summary>
		/// Viser en retningsmenu oven på fængselskortet og returnerer det valgte indeks.
		/// </summary>
		public static int Show(
			PrisonLocation currentLocation,
			string title,
			string[] options,
			string? description = null)
		{
			int selected = 0;

			while (true)
			{
				// Tegner skærmen på ny, så menuen ikke gentages nedad.
				PrisonMap.Draw(currentLocation);

				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine(title);
				Console.ResetColor();

				if (!string.IsNullOrWhiteSpace(description))
				{
					Console.WriteLine();
					Console.WriteLine(description);
				}

				Console.WriteLine();

				for (int i = 0; i < options.Length; i++)
				{
					if (i == selected)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine($"▶ {options[i]}");
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