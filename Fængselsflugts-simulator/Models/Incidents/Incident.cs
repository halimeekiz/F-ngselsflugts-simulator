using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Models.Incidents
{
	// Modelklasse: repræsenterer en hændelse i fængslet.
	// Opfylder kravet om beskrivelse, placering, alvorlighedsgrad og om hændelsen er løst.
	internal class Incident
	{
		// Callback: bliver kaldt, når hændelsen bliver løst
		public Action<Incident>? OnResolved { get; set; }
		public string Description { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
		public Severity Severity { get; set; }
		public bool IsResolved { get; private set; }

		// Markerer hændelsen som løst og kalder callbacket
		public void Resolve()
		{
			IsResolved = true;

			OnResolved?.Invoke(this);
		}
	}
}