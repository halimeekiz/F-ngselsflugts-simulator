using Fængselsflugts_simulator.Models.Prisoners;

namespace Fængselsflugts_simulator.UI
{
	/// <summary>
	/// Menu til valg af fangetype med piletaster og Enter.
	/// </summary>
	internal static class PrisonerSelection
	{
		/// <summary>
		/// Lader spilleren vælge mellem Escape Artist, Hacker og Strong Prisoner.
		/// </summary>
		public static Prisoner ChoosePrisoner()
		{
			string[] options =
			{
				"Escape Artist",
				"Hacker",
				"Strong Prisoner"
			};

			int selected = 0;

			while (true)
			{
				Console.Clear();
				Console.WriteLine("\nVÆLG DIN FANGE\n");

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

				ConsoleKey key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.UpArrow)
					selected = (selected - 1 + options.Length) % options.Length;

				if (key == ConsoleKey.DownArrow)
					selected = (selected + 1) % options.Length;

				if (key == ConsoleKey.Enter)
					break;
			}

			return selected switch
			{
				1 => new HackerPrisoner(2, "Hacker"),
				2 => new StrongPrisoner(3, "Strong"),
				_ => new EscapeArtist(1, "Escape Artist")
			};
		}
	}
}