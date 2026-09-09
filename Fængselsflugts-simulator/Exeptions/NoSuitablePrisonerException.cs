namespace Fængselsflugts_simulator.Exceptions
{
	// Selvudviklet exception: bruges når der ikke findes en egnet fange til en hændelse.
	// Sammen med PrisonerUnavailableException opfylder den kravet om mindst to egne exceptions.
	internal class NoSuitablePrisonerException : Exception
	{
		public NoSuitablePrisonerException(string message)
			: base(message)
		{
		}
	}
}