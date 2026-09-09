using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	/// <summary>
	/// Fangetype der dirker låse, hacker simple systemer og sniger sig forbi vagter.
	/// </summary>
	internal class EscapeArtist : Prisoner, ILockPicker, IHacker, ISneaky
	{
		/// <summary>
		/// Opretter en Escape Artist med power-niveau 70.
		/// </summary>
		public EscapeArtist(int id, string name)
			: base(id, name, 70)
		{
		}

		/// <inheritdoc />
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} forsøger at dirke en lås op.");
		}

		/// <inheritdoc />
		public void PickLock(Door door)
		{
			door.Open();
			Console.WriteLine($"{Name} dirker låsen på {door.Name} op.");
		}

		/// <inheritdoc />
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker et simpelt elektronisk låsesystem.");
		}

		/// <inheritdoc />
		public void Sneak()
		{
			Console.WriteLine($"{Name} sniger sig lydløst forbi vagterne.");
		}
	}
}
