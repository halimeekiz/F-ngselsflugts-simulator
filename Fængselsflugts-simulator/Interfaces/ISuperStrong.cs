using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Interfaces
{
	// Interface: beskriver en styrke-evne, som forskellige fangetyper kan implementere.
	// Det viser, at interfaces kan bruges uafhængigt af klassernes nedarvning.
	internal interface ISuperStrong
	{
		void BreakDoor(Door door);
	}
}