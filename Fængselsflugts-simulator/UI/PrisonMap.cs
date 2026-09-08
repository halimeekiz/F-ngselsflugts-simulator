namespace Fængselsflugts_simulator.UI
{
	internal class PrisonMap
	{
		public static void Draw()
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
			Console.WriteLine("║                    FÆNGSELSFLUGTS-SIMULATOR                        ║");
			Console.WriteLine("╠══════════════════════╦══════════════════════╦══════════════════════╣");

			// Celleblok A
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("║      CELLEBLOK A     ║       KANTINE        ║     CELLEBLOK B      ║");
			Console.WriteLine("║  ┌────┬────┬────┐    ║   ┌──────────────┐   ║   ┌────┬────┬────┐   ║");
			Console.WriteLine("║  │ A1 │ A2 │ A3 │    ║   │   BORDE      │   ║   │ B1 │ B2 │ B3 │   ║");
			Console.WriteLine("║  ├────┼────┼────┤    ║   │   KØKKEN     │   ║   ├────┼────┼────┤   ║");
			Console.WriteLine("║  │ A4 │ A5 │ A6 │    ║   └──────────────┘   ║   │ B4 │ B5 │ B6 │   ║");
			Console.WriteLine("║  └────┴────┴────┘    ║                      ║   └────┴────┴────┘   ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠══════════════════════╩══════════╦═══════════╩══════════════════════╣");

			// Midterområde
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("║             BAD                 ║              GÅRD                ║");
			Console.WriteLine("║   ┌───────────────┐             ║   ┌─────────────────────────┐    ║");
			Console.WriteLine("║   │ 🚿  🚿  🚿    │             ║   │                         │    ║");
			Console.WriteLine("║   │               │             ║   │      MOTIONSOMRÅDE      │    ║");
			Console.WriteLine("║   └───────────────┘             ║   │                         │    ║");
			Console.WriteLine("║                                 ║   └─────────────────────────┘    ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠══════════════╦══════════════════╬══════════════════╦═══════════════╣");

			// Nederste områder
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("║  SYGESTUE    ║     VAGTRUM      ║   KONTROLRUM     ║     LAGER     ║");
			Console.WriteLine("║   [+]        ║     [👮]         ║      [🚨]        ║     [📦]      ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╠══════════════╩══════════════════╩══════════════════╩═══════════════╣");

			// Indgang
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("║                         HOVEDINDGANG                               ║");
			Console.WriteLine("║                    ════════🚪════════                              ║");

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

			Console.ResetColor();
		}
	}
}