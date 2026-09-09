using Fængselsflugts_simulator.Interfaces;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// HackerPrisoner arver fra Prisoner og får hacking-evnen via IHacker.
	internal class HackerPrisoner : Prisoner, IHacker
	{
		// Sender fangens startdata videre til basisklassen Prisoner.
		public HackerPrisoner(int id, string name)
			: base(id, name, 60)
		{
		}

		// Override: HackerPrisoner laver sin egen version af den abstrakte metode.
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} hacker fængslets sikkerhedssystem.");
		}

		// Implementerer hacking-evnen fra IHacker.
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker sikkerhedssystemet og deaktiverer alarmen.");
		}
	}
}