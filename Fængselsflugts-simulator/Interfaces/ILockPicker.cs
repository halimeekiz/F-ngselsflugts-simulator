using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Interfaces
{
	/// <summary>
	/// Evne til at dirke eller tvinge en låst dør op. Kan implementeres uafhængigt af arv.
	/// </summary>
	internal interface ILockPicker
	{
		/// <summary>
		/// Åbner den angivne dør ved at dirke eller tvinge låsen.
		/// </summary>
		void PickLock(Door door);
	}
}
