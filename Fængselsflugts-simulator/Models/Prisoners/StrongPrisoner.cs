using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	/// <summary>
	/// Fangetype der bruger rå styrke til at bryde døre og flytte tunge forhindringer.
	/// </summary>
	internal class StrongPrisoner : Prisoner, ISuperStrong, ILockPicker, IObstacleMover
	{
		/// <summary>
		/// Opretter en Strong Prisoner med power-niveau 90.
		/// </summary>
		public StrongPrisoner(int id, string name)
			: base(id, name, 90)
		{
		}

		/// <inheritdoc />
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} sparker døren op med rå styrke!");
		}

		/// <inheritdoc />
		public void BreakDoor(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} bryder døren {door.Name} op med rå styrke.");
		}

		/// <inheritdoc />
		public void PickLock(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} tvinger låsen op med styrke.");
		}

		/// <inheritdoc />
		public void MoveObstacle(string obstacle)
		{
			Console.WriteLine($"{Name} flytter {obstacle} med rå styrke.");
		}
	}
}
