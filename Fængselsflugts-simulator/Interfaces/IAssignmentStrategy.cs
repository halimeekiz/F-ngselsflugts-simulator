using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;

namespace Fængselsflugts_simulator.Interfaces
{
	// Dependency Inversion: kontrolcentralen afhænger af dette interface
	// i stedet for en bestemt tildelingsstrategi.
	internal interface IAssignmentStrategy
	{
		Prisoner SelectPrisoner(List<Prisoner> prisoners, Incident incident);
	}
}