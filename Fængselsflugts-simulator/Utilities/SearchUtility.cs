namespace Fængselsflugts_simulator.Utilities
{
	/// <summary>
	/// Hjælpeklasse med generiske søgemetoder, der kan genbruges på forskellige collections.
	/// </summary>
	internal static class SearchUtility
	{
		/// <summary>
		/// Filtrerer en collection med et callback af typen <see cref="Func{T, Boolean}"/>.
		/// </summary>
		/// <typeparam name="T">Typen af elementer i collectionen.</typeparam>
		/// <param name="items">De elementer, der skal søges i.</param>
		/// <param name="condition">Lambda eller anden predikat-funktion, der afgør match.</param>
		/// <returns>En ny liste med de elementer, der opfylder betingelsen.</returns>
		public static List<T> Filter<T>(
			IEnumerable<T> items,
			Func<T, bool> condition)
		{
			return items.Where(condition).ToList();
		}
	}
}
