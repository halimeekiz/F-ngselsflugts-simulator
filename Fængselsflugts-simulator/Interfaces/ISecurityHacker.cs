namespace Fængselsflugts_simulator.Interfaces
{
	/// <summary>
	/// Evne til at deaktivere kameraer og alarmsystemer.
	/// </summary>
	internal interface ISecurityHacker
	{
		/// <summary>
		/// Deaktiverer overvågningskameraerne.
		/// </summary>
		void DisableCameras();

		/// <summary>
		/// Deaktiverer fængslets alarmsystem.
		/// </summary>
		void DisableAlarm();
	}
}
