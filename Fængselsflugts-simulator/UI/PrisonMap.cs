using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.UI
{
	/// <summary>
	/// Tegner det visuelle fængselskort og markerer spillerens placering.
	/// </summary>
	internal class PrisonMap
	{
		/// <summary>
		/// Tegner kortet og fremhæver det rum, spilleren står i.
		/// </summary>
		public static void Draw(PrisonLocation currentLocation)
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                    FÆNGSELSFLUGTS-SIMULATOR                        ║");
			Console.WriteLine("╠══════════════════════╦══════════════════════╦══════════════════════╣");

			// Øverste områder
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(
				$"║{MarkLocation(currentLocation, PrisonLocation.CellBlockA, "CELLEBLOK A", 22)}" +
				$"║{MarkLocation(currentLocation, PrisonLocation.Cafeteria, "KANTINE", 22)}" +
				$"║{MarkLocation(currentLocation, PrisonLocation.CellBlockB, "CELLEBLOK B", 22)}║");

			Console.WriteLine("║  ┌────┬────┬────┐    ║   ┌──────────────┐   ║   ┌────┬────┬────┐   ║");
			Console.WriteLine("║  │ A1 │ A2 │ A3 │    ║   │   BORDE      │   ║   │ B1 │ B2 │ B3 │   ║");
			Console.WriteLine("║  ├────┼────┼────┤    ║   │   KØKKEN     │   ║   ├────┼────┼────┤   ║");
			Console.WriteLine("║  │ A4 │ A5 │ A6 │    ║   └──────────────┘   ║   │ B4 │ B5 │ B6 │   ║");
			Console.WriteLine("║  └────┴────┴────┘    ║                      ║   └────┴────┴────┘   ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠══════════════════════╩══════════════════════╩══════════════════════╣");

			// Gang
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(
				$"║{MarkLocation(currentLocation, PrisonLocation.Corridor, "GANG", 68)}║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠═════════════════════════════════╦══════════════════════════════════╣");

			// Midterområder
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(
				$"║{MarkLocation(currentLocation, PrisonLocation.Shower, "BAD", 33)}" +
				$"║{MarkLocation(currentLocation, PrisonLocation.Yard, "GÅRD", 34)}║");

			Console.WriteLine("║   ┌───────────────┐             ║   ┌─────────────────────────┐    ║");
			Console.WriteLine("║   │ 🚿  🚿  🚿    │             ║   │                         │    ║");
			Console.WriteLine("║   │               │             ║   │      MOTIONSOMRÅDE      │    ║");
			Console.WriteLine("║   └───────────────┘             ║   │                         │    ║");
			Console.WriteLine("║                                 ║   └─────────────────────────┘    ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠══════════════╦══════════════════╬══════════════════╦═══════════════╣");

			// Nederste områder
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine(
				$"║{MarkLocation(currentLocation, PrisonLocation.Medical, "SYGESTUE", 14)}" +
				$"║{MarkLocation(currentLocation, PrisonLocation.GuardRoom, "VAGTRUM", 18)}" +
				$"║{MarkLocation(currentLocation, PrisonLocation.ControlRoom, "KONTROLRUM", 18)}" +
				$"║{MarkLocation(currentLocation, PrisonLocation.Storage, "LAGER", 15)}║");

			Console.WriteLine("║     [+]      ║       [👮]       ║       [🚨]       ║      [📦]     ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠══════════════╩══════════════════╩══════════════════╩═══════════════╣");

			// Hovedindgang
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(
				$"║{MarkLocation(currentLocation, PrisonLocation.MainEntrance, "HOVEDINDGANG", 68)}║");

			Console.WriteLine("║                    ════════🚪════════                              ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"● DU ER HER: {GetLocationName(currentLocation)}");
			Console.ResetColor();
		}

		// Markerer spillerens aktuelle placering direkte på kortet.
		private static string MarkLocation(
			PrisonLocation currentLocation,
			PrisonLocation location,
			string name,
			int width)
		{
			string text = currentLocation == location
				? $"● {name}"
				: name;

			int padding = width - text.Length;
			int leftPadding = padding / 2;
			int rightPadding = padding - leftPadding;

			return new string(' ', leftPadding)
				+ text
				+ new string(' ', rightPadding);
		}

		// Oversætter spillerens position til et læsbart navn på kortet.
		private static string GetLocationName(PrisonLocation location)
		{
			return location switch
			{
				PrisonLocation.CellBlockA => "Celleblok A",
				PrisonLocation.CellBlockB => "Celleblok B",
				PrisonLocation.Corridor => "Gang",
				PrisonLocation.Cafeteria => "Kantine",
				PrisonLocation.Shower => "Bad",
				PrisonLocation.Yard => "Gård",
				PrisonLocation.Medical => "Sygestue",
				PrisonLocation.GuardRoom => "Vagtrum",
				PrisonLocation.ControlRoom => "Kontrolrum",
				PrisonLocation.Storage => "Lager",
				PrisonLocation.MainEntrance => "Hovedindgang",
				_ => "Ukendt"
			};
		}
	}
}