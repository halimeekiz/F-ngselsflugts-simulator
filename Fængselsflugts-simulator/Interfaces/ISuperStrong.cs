using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Interfaces
{
	/// <summary>
	/// Evne til at bryde døre op med fysisk styrke.
	/// </summary>
	internal interface ISuperStrong
	{
		/// <summary>
		/// Bryder den angivne dør op med rå styrke.
		/// </summary>
		void BreakDoor(Door door);
	}
}
