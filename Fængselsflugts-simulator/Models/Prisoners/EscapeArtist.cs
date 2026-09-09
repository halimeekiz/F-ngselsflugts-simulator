using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// EscapeArtist arver fra Prisoner og implementerer flere interfaces.
	internal class EscapeArtist : Prisoner, ILockPicker, IHacker, ISneaky
	{
		// Sender fangens startdata videre til basisklassen Prisoner.
		public EscapeArtist(int id, string name)
			: base(id, name, 70)
		{
		}

		// Override: EscapeArtist laver sin egen version af den abstrakte metode.
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} forsøger at dirke en lås op.");
		}

		// Implementerer låsedirknings-evnen fra ILockPicker.
		public void PickLock(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} dirker låsen på {door.Name} op.");
		}

		// Implementerer hacking-evnen fra IHacker.
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker et simpelt elektronisk låsesystem.");
		}

		// Implementerer snigeevnen fra ISneaky.
		public void Sneak()
		{
			Console.WriteLine($"{Name} sniger sig lydløst forbi vagterne.");
		}
	}
}