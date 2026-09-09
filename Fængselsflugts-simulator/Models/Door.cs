namespace Fængselsflugts_simulator.Models
{
	// Repræsenterer en dør, som kan låses, åbnes eller påvirkes af fangernes evner
	internal class Door
	{
		public string Name { get; set; } = string.Empty;
		public bool IsLocked { get; private set; } = true;

		public void Open()
		{
			IsLocked = false;
		}

		public void Lock()
		{
			IsLocked = true;
		}
	}
}