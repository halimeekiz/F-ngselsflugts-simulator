using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Models.Incidents
{
	/// <summary>
	/// En hændelse i fængslet med beskrivelse, placering, alvorlighed og løsningsstatus.
	/// </summary>
	internal class Incident
	{
		/// <summary>
		/// Callback der kaldes, når hændelsen bliver markeret som løst.
		/// </summary>
		public Action<Incident>? OnResolved { get; set; }

		public string Description { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;
		public Severity Severity { get; set; }

		/// <summary>
		/// Angiver om hændelsen er løst. Kan kun ændres via <see cref="Resolve"/>.
		/// </summary>
		public bool IsResolved { get; private set; }

		/// <summary>
		/// Markerer hændelsen som løst og udløser <see cref="OnResolved"/>.
		/// </summary>
		public void Resolve()
		{
			IsResolved = true;

			OnResolved?.Invoke(this);
		}
	}
}
