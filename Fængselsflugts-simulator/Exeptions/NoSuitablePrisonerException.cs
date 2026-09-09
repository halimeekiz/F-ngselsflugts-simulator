namespace Fængselsflugts_simulator.Exceptions
{
	/// <summary>
	/// Kastes, når der ikke findes en egnet ledig fange til en hændelse.
	/// </summary>
	internal class NoSuitablePrisonerException : Exception
	{
		/// <summary>
		/// Opretter exceptionen med en forklarende besked.
		/// </summary>
		public NoSuitablePrisonerException(string message)
			: base(message)
		{
		}
	}
}
