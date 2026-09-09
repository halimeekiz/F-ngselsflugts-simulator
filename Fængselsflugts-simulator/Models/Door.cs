namespace Fængselsflugts_simulator.Models
{
	/// <summary>
	/// En dør, som fanger kan åbne med forskellige evner.
	/// </summary>
	internal class Door
	{
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Angiver om døren er låst. Ændres kun via <see cref="Open"/> og <see cref="Lock"/>.
		/// </summary>
		public bool IsLocked { get; private set; } = true;

		/// <summary>
		/// Åbner døren.
		/// </summary>
		public void Open()
		{
			IsLocked = false;
		}

		/// <summary>
		/// Låser døren.
		/// </summary>
		public void Lock()
		{
			IsLocked = true;
		}
	}
}
