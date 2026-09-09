using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Flere interfaces: EscapeArtist har både evnen til at dirke låse og hacke.
	// Det viser, at en klasse kan implementere flere interfaces.
	internal class EscapeArtist : Prisoner, ILockPicker, IHacker
	{
		// Override: fangetypen laver sin egen version af den abstrakte metode
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} forsøger at dirke en lås op.");
		}

		// Implementerer den evne, som ILockPicker kræver
		public void PickLock(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} dirker låsen på {door.Name} op.");
		}

		// Implementerer hacking-evnen fra IHacker
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker et simpelt elektronisk låsesystem.");
		}

		public EscapeArtist()
		{
			PowerLevel = 70;
			IsAvailable = true;
		}
	}
}