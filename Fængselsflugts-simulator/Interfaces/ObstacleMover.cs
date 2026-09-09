namespace Fængselsflugts_simulator.Interfaces
{
	/// <summary>
	/// Evne til at flytte tunge forhindringer.
	/// </summary>
	internal interface IObstacleMover
	{
		/// <summary>
		/// Flytter den angivne forhindring med rå styrke.
		/// </summary>
		void MoveObstacle(string obstacle);
	}
}
