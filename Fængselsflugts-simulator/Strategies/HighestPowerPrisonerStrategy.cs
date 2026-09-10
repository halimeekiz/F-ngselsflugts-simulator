using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;

namespace Fængselsflugts_simulator.Strategies
{
	/// <summary>
	/// Konkret tildelingsstrategi, der vælger den ledige fange med højest power.
	/// </summary>
	internal class HighestPowerPrisonerStrategy : IAssignmentStrategy
	{
		/// <inheritdoc />
		public Prisoner SelectPrisoner(
			List<Prisoner> prisoners,
			Incident incident)
		{
			Prisoner? prisoner = prisoners
				.Where(p => p.IsAvailable)
				.OrderByDescending(p => p.PowerLevel)
				.FirstOrDefault();

			if (prisoner == null)
			{
				throw new NoSuitablePrisonerException(
					"Der findes ingen ledige fanger til hændelsen.");
			}

			return prisoner;
		}
	}
}
