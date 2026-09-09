namespace Fængselsflugts_simulator.Interfaces
{
	// Interface: evner til at manipulere fængslets sikkerhedssystem.
	internal interface ISecurityHacker
	{
		void DisableCameras();
		void DisableAlarm();
	}
}