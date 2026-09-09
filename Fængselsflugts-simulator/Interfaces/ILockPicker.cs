using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Interfaces
{
	// Interface: beskriver en evne, som kun nogle fangetyper har.
	// Interfaces gør, at evnen kan bruges på tværs af forskellige fangetyper.
	internal interface ILockPicker
	{
		void PickLock(Door door);
	}
}