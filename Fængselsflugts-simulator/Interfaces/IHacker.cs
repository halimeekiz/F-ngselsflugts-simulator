namespace Fængselsflugts_simulator.Interfaces
{
	// Interface: beskriver en hacking-evne, som kan implementeres af forskellige fangetyper.
	// Det opfylder kravet om evner via interfaces uafhængigt af arv.
	internal interface IHacker
	{
		void HackSecurity();
	}
}