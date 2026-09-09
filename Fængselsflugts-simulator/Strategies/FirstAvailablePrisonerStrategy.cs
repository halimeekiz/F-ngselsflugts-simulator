using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;

namespace Fængselsflugts_simulator.Strategies
{
	/// <summary>
	/// Konkret tildelingsstrategi, der vælger den første ledige fange.
	/// </summary>
	internal class FirstAvailablePrisonerStrategy : IAssignmentStrategy
	{
		/// <inheritdoc />
		public Prisoner SelectPrisoner(
			List<Prisoner> prisoners,
			Incident incident)
		{
			Prisoner? prisoner =
				prisoners.FirstOrDefault(p => p.IsAvailable);

			if (prisoner == null)
			{
				throw new NoSuitablePrisonerException(
					"Der findes ingen ledige fanger til hændelsen.");
			}

			return prisoner;
		}
	}
}
