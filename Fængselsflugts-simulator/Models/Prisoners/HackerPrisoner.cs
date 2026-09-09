using Fængselsflugts_simulator.Interfaces;

namespace Fængselsflugts_simulator.Models.Prisoners
{
	/// <summary>
	/// Fangetype der hacker sikkerhedssystemer, kameraer og alarmer.
	/// </summary>
	internal class HackerPrisoner : Prisoner, IHacker, ISecurityHacker
	{
		/// <summary>
		/// Opretter en hacker med power-niveau 60.
		/// </summary>
		public HackerPrisoner(int id, string name)
			: base(id, name, 60)
		{
		}

		/// <inheritdoc />
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} bruger sine hacking-evner.");
		}

		/// <inheritdoc />
		public void HackSecurity()
		{
			Console.WriteLine($"{Name} hacker sikkerhedssystemet.");
		}

		/// <inheritdoc />
		public void DisableCameras()
		{
			Console.WriteLine($"{Name} deaktiverer overvågningskameraerne.");
		}

		/// <inheritdoc />
		public void DisableAlarm()
		{
			Console.WriteLine($"{Name} deaktiverer alarmsystemet.");
		}
	}
}
