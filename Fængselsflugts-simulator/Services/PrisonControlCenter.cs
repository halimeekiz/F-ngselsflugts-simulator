using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.Utilities;
using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;

namespace Fængselsflugts_simulator.Services
{
	// Vagtcentralen holder styr på alle registrerede fanger og hændelser.
	// List bruges som collection, fordi vi har brug for at gemme flere objekter af samme type.
	internal class PrisonControlCenter
	{
		private readonly List<Prisoner> prisoners = new();
		private readonly List<Incident> incidents = new();
		// Dependency Inversion: vi afhænger af interfacet og ikke en konkret strategi
		private readonly IAssignmentStrategy assignmentStrategy;

		// Constructor injection: strategien bliver givet udefra.
		// Det gør det nemt at udskifte strategien uden at ændre kontrolcentralen.
		public PrisonControlCenter(IAssignmentStrategy assignmentStrategy)
		{
			this.assignmentStrategy = assignmentStrategy;
		}

		// Tilføjer en fange til vores collection
		public void RegisterPrisoner(Prisoner prisoner)
		{
			prisoners.Add(prisoner);
		}

		// Tilføjer en hændelse til vores collection
		public void ReportIncident(Incident incident)
		{
			incidents.Add(incident);
		}

		// Gennemgår collectionen og viser alle indmeldte hændelser
		public void ShowIncidents()
		{
			foreach (Incident incident in incidents)
			{
				Console.WriteLine(
					$"{incident.Description} | Sted: {incident.Location} | " +
					$"Alvorlighed: {incident.Severity} | Løst: {incident.IsResolved}");
			}
		}

		// Exception-håndtering: hvis ingen ledig fange findes, kastes vores egen exception.
		public Prisoner GetFirstAvailablePrisoner()
		{
			Prisoner? prisoner = prisoners.FirstOrDefault(p => p.IsAvailable);

			if (prisoner == null)
			{
				throw new NoSuitablePrisonerException(
					"Der blev ikke fundet en ledig fange.");
			}

			return prisoner;
		}

		// Generics: samme Filter<T>-metode bruges på Incident-collectionen
		public List<Incident> GetUnresolvedIncidents()
		{
			return SearchUtility.Filter(incidents, incident => !incident.IsResolved);
		}

		// Exception-håndtering: en optaget fange må ikke tildeles en ny hændelse
		public void CheckPrisonerAvailability(Prisoner prisoner)
		{
			if (!prisoner.IsAvailable)
			{
				throw new PrisonerUnavailableException(
					$"{prisoner.Name} er allerede optaget.");
			}
		}

		// Generics: samme Filter<T>-metode bruges på Prisoner-collectionen
		public List<Prisoner> GetAvailablePrisoners()
		{
			return SearchUtility.Filter(prisoners, prisoner => prisoner.IsAvailable);
		}

		// Dependency Inversion: bruger den strategi, som blev givet via constructoren.
		public Prisoner AssignPrisoner(Incident incident)
		{
			Prisoner prisoner = assignmentStrategy.SelectPrisoner(prisoners, incident);

			CheckPrisonerAvailability(prisoner);

			prisoner.IsAvailable = false;

			return prisoner;
		}
	}
}