using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// StrongPrisoner arver fra Prisoner og implementerer flere interfaces.
	internal class StrongPrisoner : Prisoner, ISuperStrong, ILockPicker
	{
		// Sender fangens startdata videre til basisklassen Prisoner.
		public StrongPrisoner(int id, string name)
			: base(id, name, 90)
		{
		}

		// Override: StrongPrisoner laver sin egen version af den abstrakte metode.
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} sparker døren op med rå styrke!");
		}

		// Implementerer styrke-evnen fra ISuperStrong.
		public void BreakDoor(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} bryder døren {door.Name} op med rå styrke.");
		}

		// Implementerer låsedirknings-evnen fra ILockPicker.
		public void PickLock(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} tvinger låsen op med styrke.");
		}
	}
}