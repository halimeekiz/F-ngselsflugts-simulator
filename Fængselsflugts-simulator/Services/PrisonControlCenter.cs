using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.Utilities;
using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;

namespace Fængselsflugts_simulator.Services
{
	/// <summary>
	/// Fængslets kontrolcentral. Holder registrerede fanger og hændelser i collections
	/// og tildeler fanger via en udskiftelig strategi.
	/// </summary>
	internal class PrisonControlCenter
	{
		private readonly List<Prisoner> prisoners = new();
		private readonly List<Incident> incidents = new();

		/// <summary>
		/// Tildelingsstrategien injectes udefra, så klassen ikke afhænger af en konkret implementation.
		/// </summary>
		private readonly IAssignmentStrategy assignmentStrategy;

		/// <summary>
		/// Opretter kontrolcentralen med constructor injection af tildelingsstrategien.
		/// </summary>
		public PrisonControlCenter(IAssignmentStrategy assignmentStrategy)
		{
			this.assignmentStrategy = assignmentStrategy;
		}

		/// <summary>
		/// Registrerer en fange i collectionen.
		/// </summary>
		public void RegisterPrisoner(Prisoner prisoner)
		{
			prisoners.Add(prisoner);
		}

		/// <summary>
		/// Indmelder en hændelse i collectionen.
		/// </summary>
		public void ReportIncident(Incident incident)
		{
			incidents.Add(incident);
		}

		/// <summary>
		/// Viser alle indmeldte hændelser i konsollen.
		/// </summary>
		public void ShowIncidents()
		{
			if (incidents.Count == 0)
			{
				Console.WriteLine("Ingen hændelser er registreret endnu.");
				return;
			}

			foreach (Incident incident in incidents)
			{
				Console.WriteLine(
					$"{incident.Description} | Sted: {incident.Location} | " +
					$"Alvorlighed: {incident.Severity} | Løst: {incident.IsResolved}");
			}
		}

		/// <summary>
		/// Finder den første ledige fange.
		/// </summary>
		/// <exception cref="NoSuitablePrisonerException">
		/// Kastes, hvis ingen ledig fange findes.
		/// </exception>
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

		/// <summary>
		/// Returnerer uafklarede hændelser via den generiske <see cref="SearchUtility.Filter{T}"/>.
		/// </summary>
		public List<Incident> GetUnresolvedIncidents()
		{
			return SearchUtility.Filter(incidents, incident => !incident.IsResolved);
		}

		/// <summary>
		/// Kontrollerer, at fangen er ledig.
		/// </summary>
		/// <exception cref="PrisonerUnavailableException">
		/// Kastes, hvis fangen allerede er optaget.
		/// </exception>
		public void CheckPrisonerAvailability(Prisoner prisoner)
		{
			if (!prisoner.IsAvailable)
			{
				throw new PrisonerUnavailableException(
					$"{prisoner.Name} er allerede optaget.");
			}
		}

		/// <summary>
		/// Returnerer ledige fanger via den generiske <see cref="SearchUtility.Filter{T}"/>.
		/// </summary>
		public List<Prisoner> GetAvailablePrisoners()
		{
			return SearchUtility.Filter(prisoners, prisoner => prisoner.IsAvailable);
		}

		/// <summary>
		/// Tildeler en fange til hændelsen ved hjælp af den injectede strategi.
		/// </summary>
		public Prisoner AssignPrisoner(Incident incident)
		{
			Prisoner prisoner = assignmentStrategy.SelectPrisoner(prisoners, incident);

			CheckPrisonerAvailability(prisoner);

			prisoner.IsAvailable = false;

			return prisoner;
		}
	}
}
