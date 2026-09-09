namespace Fængselsflugts_simulator.Interfaces
{
	/// <summary>
	/// Evne til at hacke elektroniske sikkerhedssystemer.
	/// </summary>
	internal interface IHacker
	{
		/// <summary>
		/// Hacker et sikkerhedssystem, så en lås eller port kan åbnes.
		/// </summary>
		void HackSecurity();
	}
}
