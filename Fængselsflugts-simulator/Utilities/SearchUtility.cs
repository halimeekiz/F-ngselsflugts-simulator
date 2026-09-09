namespace Fængselsflugts_simulator.Utilities
{
	// Generisk metode: T gør, at samme søgemetode kan bruges på forskellige typer.
	// Func<T, bool> bruges som callback til at bestemme, hvilke elementer der matcher.
	internal static class SearchUtility
	{
		public static List<T> Filter<T>(
			IEnumerable<T> items,
			Func<T, bool> condition)
		{
			return items.Where(condition).ToList();
		}
	}
}