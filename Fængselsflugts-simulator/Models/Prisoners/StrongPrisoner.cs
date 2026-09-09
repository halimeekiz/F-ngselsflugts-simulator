namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Arv: StrongPrisoner er en konkret fangetype, som arver fra Prisoner.
	internal class StrongPrisoner : Prisoner
	{
		// Konstruktør sætter fangens styrkeniveau.
		public StrongPrisoner()
		{
			PowerLevel = 90;
		}

		// Override: denne fangetype bruger sin styrke til at bryde en dør op.
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} sparker døren op med rå styrke!");
		}
	}
}