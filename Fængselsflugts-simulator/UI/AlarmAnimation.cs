namespace Fængselsflugts_simulator.UI
{
	/// <summary>
	/// Konsolanimation, der vises når fængslets alarm bliver aktiveret.
	/// </summary>
	internal static class AlarmAnimation
	{
		/// <summary>
		/// Afspiller alarm-animationen i konsollen.
		/// </summary>
		public static void Show()
		{
			for (int i = 0; i < 4; i++)
			{
				Console.Clear();

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine();
				Console.WriteLine("╔══════════════════════════════════════╗");
				Console.WriteLine("║          🚨  ALARM!  🚨              ║");
				Console.WriteLine("║                                      ║");
				Console.WriteLine("║       FLUGTFORSØG OPDAGET!           ║");
				Console.WriteLine("╚══════════════════════════════════════╝");
				Console.ResetColor();

				Thread.Sleep(350);

				Console.Clear();
				Thread.Sleep(200);
			}

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("🚨 ALARMEN ER AKTIVERET! 🚨");
			Console.ResetColor();
		}
	}
}