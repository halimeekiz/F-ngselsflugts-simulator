using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;

namespace Fængselsflugts_simulator.Interfaces
{
	/// <summary>
	/// Strategi for tildeling af fanger til hændelser.
	/// PrisonControlCenter afhænger af dette interface og ikke af en konkret klasse.
	/// </summary>
	internal interface IAssignmentStrategy
	{
		/// <summary>
		/// Vælger en fange til den angivne hændelse.
		/// </summary>
		Prisoner SelectPrisoner(List<Prisoner> prisoners, Incident incident);
	}
}
