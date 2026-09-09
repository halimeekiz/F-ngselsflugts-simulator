using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.Utilities;

namespace Fængselsflugts_simulator.Services
{
	// Vagtcentralen holder styr på alle registrerede fanger og hændelser.
	// List bruges som collection, fordi vi har brug for at gemme flere objekter af samme type.
	internal class PrisonControlCenter
	{
		private readonly List<Prisoner> prisoners = new();
		private readonly List<Incident> incidents = new();

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

		// Generics: samme Filter<T>-metode bruges på Prisoner-collectionen
		public List<Prisoner> GetAvailablePrisoners()
		{
			return SearchUtility.Filter(prisoners, prisoner => prisoner.IsAvailable);
		}

		// Generics: samme Filter<T>-metode bruges på Incident-collectionen
		public List<Incident> GetUnresolvedIncidents()
		{
			return SearchUtility.Filter(incidents, incident => !incident.IsResolved);
		}
	}
}