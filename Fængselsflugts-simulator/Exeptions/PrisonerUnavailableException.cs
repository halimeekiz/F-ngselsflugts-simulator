namespace Fængselsflugts_simulator.Exceptions
{
	/// <summary>
	/// Kastes, når en fange ikke er ledig til en opgave.
	/// </summary>
	internal class PrisonerUnavailableException : Exception
	{
		/// <summary>
		/// Opretter exceptionen med en forklarende besked.
		/// </summary>
		public PrisonerUnavailableException(string message)
			: base(message)
		{
		}
	}
}
