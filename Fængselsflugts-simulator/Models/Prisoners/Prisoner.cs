using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Abstrakt basisklasse: fælles grundlag for alle fangetyper.
	// Bruges til arv, override og polymorfi.
	internal abstract class Prisoner
	{
		// Indkapsling: powerLevel kan ikke ændres direkte udefra.
		private int powerLevel;

		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public PrisonerStatus Status { get; set; }
		public bool IsAvailable { get; set; }

		public int PowerLevel
		{
			get { return powerLevel; }

			// protected set betyder, at kun Prisoner og underklasser kan sætte værdien.
			protected set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				powerLevel = value;
			}
		}

		// protected constructor: kun Prisoner og dens underklasser kan bruge den.
		protected Prisoner(int id, string name, int powerLevel)
		{
			Id = id;
			Name = name;
			PowerLevel = powerLevel;
			Status = PrisonerStatus.InCell;
			IsAvailable = true;
		}

		// Abstrakt metode: hver fangetype skal lave sin egen version.
		public abstract void PerformSpecialAction();
	}
}