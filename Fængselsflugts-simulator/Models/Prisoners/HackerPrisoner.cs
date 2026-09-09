using Fængselsflugts_simulator.Interfaces;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// HackerPrisoner arver fra Prisoner og har flere hacking-evner.
	internal class HackerPrisoner : Prisoner, IHacker, ISecurityHacker
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

		// Implementerer almindelig hacking fra IHacker.
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker sikkerhedssystemet.");
		}

		// Implementerer sikkerhedsevne fra ISecurityHacker.
		public void DisableCameras()
		{
			Console.WriteLine($"{Name} deaktiverer overvågningskameraerne.");
		}

		// Implementerer sikkerhedsevne fra ISecurityHacker.
		public void DisableAlarm()
		{
			Console.WriteLine($"{Name} deaktiverer alarmsystemet.");
		}
	}
}