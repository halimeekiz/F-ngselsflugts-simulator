namespace Fængselsflugts_simulator.UI
{
	// Viser spillets intro og mission.
	internal static class GameIntro
	{
		public static void Show()
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("╔════════════════════════════════════════════╗");
			Console.WriteLine("║        FÆNGSELSFLUGTS-SIMULATOR            ║");
			Console.WriteLine("╚════════════════════════════════════════════╝");
			Console.ResetColor();

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("MISSION:");
			Console.ResetColor();

			Console.WriteLine("Du er fanget i et topsikret fængsel.");
			Console.WriteLine("Find en vej ud uden at blive opdaget.");

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("FORHINDRINGER:");
			Console.ResetColor();

			Console.WriteLine("• Låste døre");
			Console.WriteLine("• Sikkerhedssystemer");
			Console.WriteLine("• Vagter");

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Din fangetype bestemmer dine evner.");
			Console.ResetColor();

			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("▶ Tryk ENTER for at fortsætte");
			Console.ResetColor();

			while (Console.ReadKey(true).Key != ConsoleKey.Enter)
			{
			}
		}
	}
}