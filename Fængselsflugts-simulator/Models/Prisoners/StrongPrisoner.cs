using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Flere interfaces: StrongPrisoner har både superstyrke og evnen til at dirke låse.
	// Det viser, at samme klasse kan implementere flere interfaces.
	internal class StrongPrisoner : Prisoner, ISuperStrong, ILockPicker
	{
		// Konstruktør sætter fangens styrkeniveau.
		public StrongPrisoner()
		{
			PowerLevel = 90;
		}

		// Override: denne fangetype bruger sin styrke til at bryde en dør op.
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} sparker døren op med rå styrke!");
		}

		// Implementerer den evne, som ISuperStrong kræver
		public void BreakDoor(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} bryder døren {door.Name} op med rå styrke.");
		}
		// Implementerer låsedirknings-evnen fra ILockPicker
		public void PickLock(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} tvinger låsen op med styrke.");
		}
	}
}