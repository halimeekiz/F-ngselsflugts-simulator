using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Abstrakt basisklasse: fælles grundlag for alle fangetyper og bruges til arv og polymorfi
	internal abstract class Prisoner
	{
		// Indkapsling: feltet er private, så powerLevel ikke kan ændres direkte udefra
		private int powerLevel;

		public int Id { get; set; }
		public string Name { get; set; } = string.Empty; 
		public PrisonerStatus Status { get; set; }
		public bool IsAvailable { get; set; }

		public int PowerLevel
		{
			get { return powerLevel; }

			// protected gør, at kun Prisoner og dens underklasser kan sætte værdien
			protected set
			{
				// Sikrer at PowerLevel altid er mellem 0 og 100
				if (value < 0 || value > 100)
					throw new ArgumentOutOfRangeException(nameof(value));

				powerLevel = value;
			}
		}
		// Abstrakt metode: tvinger alle konkrete fangetyper til at lave deres egen version.
		// Det bruges senere til at demonstrere override og polymorfi.
		public abstract void PerformSpecialAction();
	}

}
