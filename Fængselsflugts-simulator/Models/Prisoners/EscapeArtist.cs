namespace Fængselsflugts_simulator.Models.Prisoners
{
	// Arv: EscapeArtist er en konkret fangetype, som arver fra Prisoner
	internal class EscapeArtist : Prisoner
	{
		// Override: fangetypen laver sin egen version af den abstrakte metode
		public override void PerformSpecialAction()
		{
			Console.WriteLine($"{Name} forsøger at dirke en lås op.");
		}
	}
}