using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	/// <summary>
	/// Abstrakt basisklasse for alle fangetyper.
	/// Indeholder fælles data og den abstrakte metode, som underklasserne overrider.
	/// </summary>
	internal abstract class Prisoner
	{
		/// <summary>
		/// Indkapslet power-værdi. Kan ikke sættes direkte udefra.
		/// </summary>
		private int powerLevel;

		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public PrisonerStatus Status { get; set; }
		public bool IsAvailable { get; set; }

		/// <summary>
		/// Fangens energi-/power-niveau mellem 0 og 100.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Kastes, hvis værdien ligger uden for 0-100.
		/// </exception>
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

		/// <summary>
		/// Beskyttet constructor, så kun underklasser kan oprette en fange.
		/// </summary>
		protected Prisoner(int id, string name, int powerLevel)
		{
			Id = id;
			Name = name;
			PowerLevel = powerLevel;
			Status = PrisonerStatus.InCell;
			IsAvailable = true;
		}

		/// <summary>
		/// Udfører fangetypens særlige handling. Implementeres forskelligt i underklasserne.
		/// </summary>
		public abstract void PerformSpecialAction();
	}
}
