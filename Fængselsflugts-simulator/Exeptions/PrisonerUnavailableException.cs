namespace Fængselsflugts_simulator.Exceptions
{
	// Selvudviklet exception: bruges når en fange ikke er ledig til en opgave.
	internal class PrisonerUnavailableException : Exception
	{
		public PrisonerUnavailableException(string message)
			: base(message)
		{
		}
	}
}