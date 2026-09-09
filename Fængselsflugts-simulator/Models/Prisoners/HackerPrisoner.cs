using Fængselsflugts_simulator.Interfaces;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Interface-implementering: HackerPrisoner arver fra Prisoner og får samtidig hacking-evnen via IHacker
	internal class HackerPrisoner : Prisoner, IHacker
	{
		// Override: HackerPrisoner laver sin egen version af den abstrakte metode
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} hacker fængslets sikkerhedssystem.");
		}

		// Implementerer den evne, som IHacker kræver
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker sikkerhedssystemet og deaktiverer alarmen.");
		}
	}
}