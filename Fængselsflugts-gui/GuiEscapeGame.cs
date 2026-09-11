using Fængselsflugts_simulator.Enums;
using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;
using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.Services;

namespace Fængselsflugts_simulator.Gui;

internal sealed class GuiEscapeGame
{
	private readonly PrisonControlCenter controlCenter;
	private readonly Action<string> log;
	private Prisoner player = null!;
	private PrisonLocation currentLocation;
	private bool escaping;
	private bool alarmTriggered;
	private bool cratesMoved;
	private bool yardCleared;
	private bool guardRoomCleared;
	private bool securityDisabled;
	private bool kitchenHacked;
	private bool lockerPicked;
	private Incident? escapeIncident;
	private Incident? alarmIncident;
	private List<(string Label, Action Act)> choices = new();

	public GuiEscapeGame(PrisonControlCenter controlCenter, Action<string> log)
	{
		this.controlCenter = controlCenter;
		this.log = log;
	}

	public PrisonLocation Location => currentLocation;
	public bool AlarmOn => alarmTriggered;
	public bool Finished => !escaping;
	public string Title { get; private set; } = "FLUGTEN";
	public IReadOnlyList<string> ChoiceLabels => choices.Select(c => c.Label).ToList();

	public event Action? Changed;

	public void Start(Prisoner chosenPlayer)
	{
		player = chosenPlayer;
		player.Status = PrisonerStatus.Escaping;
		player.IsAvailable = false;
		currentLocation = PrisonLocation.CellBlockA;
		alarmTriggered = false;
		cratesMoved = false;
		yardCleared = false;
		guardRoomCleared = false;
		securityDisabled = false;
		kitchenHacked = false;
		lockerPicked = false;
		escapeIncident = null;
		alarmIncident = null;
		escaping = true;

		Title = "FLUGTEN BEGYNDER";
		log("Du sidder i din celle.");
		log("Foran dig er en låst celledør.");
		log("Find vejen til hovedindgangen, og kom ud.");
		SetChoices(("Åbn celledøren", OpenCell));
	}

	public void Pick(int index)
	{
		if (index < 0 || index >= choices.Count || !escaping)
		{
			return;
		}

		choices[index].Act();
	}

	private void OpenCell()
	{
		Door cellDoor = new() { Name = "celledøren" };
		cellDoor.Lock();
		OpenLockedDoor(cellDoor);

		if (!escaping)
		{
			return;
		}

		log("Du er ude af cellen!");
		NotifyControlCenterOfEscape();
		SetChoices(("Fortsæt ud på gangen", () =>
		{
			currentLocation = PrisonLocation.Corridor;
			ShowCorridor();
		}));
	}

	private void OpenLockedDoor(Door door)
	{
		if (!door.IsLocked)
		{
			log($"{door.Name} er allerede åben.");
			return;
		}

		player.PerformSpecialAction();

		if (player is ISuperStrong strongPrisoner)
		{
			strongPrisoner.BreakDoor(door);
		}
		else if (player is ILockPicker lockPicker)
		{
			lockPicker.PickLock(door);
		}
		else if (player is IHacker hacker)
		{
			hacker.HackSecurity();
			door.Open();
		}
		else
		{
			GetCaught($"Du har ingen evne til at åbne {door.Name}.");
		}
	}

	private void ShowCorridor()
	{
		Title = "DU ER PÅ GANGEN";
		log("Gangen forbinder fængslets områder. Hovedindgangen ligger i den anden ende.");
		SetChoices(
			("← Celleblok A", () => Go(PrisonLocation.CellBlockA)),
			("↑ Kantinen", () => Go(PrisonLocation.Cafeteria)),
			("→ Celleblok B", () => Go(PrisonLocation.CellBlockB)),
			("↓ Badet", () => Go(PrisonLocation.Shower)),
			("↓ Gården", () => Go(PrisonLocation.Yard)));
	}

	private void Go(PrisonLocation location)
	{
		currentLocation = location;
		EnterRoom();
	}

	private void EnterRoom()
	{
		switch (currentLocation)
		{
			case PrisonLocation.CellBlockA:
				Title = "DU ER I CELLEBLOK A";
				log("Celledøren står åben bag dig.");
				SetChoices(("→ Gangen", () => Go(PrisonLocation.Corridor)));
				break;
			case PrisonLocation.Corridor:
				ShowCorridor();
				break;
			case PrisonLocation.Cafeteria:
				HandleCafeteria();
				break;
			case PrisonLocation.CellBlockB:
				HandleCellBlockB();
				break;
			case PrisonLocation.Shower:
				Title = "DU ER I BADDET";
				log("Badet er stille. En bagvej fører videre mod sygestuen.");
				SetChoices(
					("↑ Gangen", () => Go(PrisonLocation.Corridor)),
					("↓ Sygestuen", () => Go(PrisonLocation.Medical)));
				break;
			case PrisonLocation.Yard:
				HandleYard();
				break;
			case PrisonLocation.Medical:
				Title = "DU ER PÅ SYGESTUEN";
				log("Sygestuen er tom. Herfra kan du komme ind i vagtrummet.");
				SetChoices(
					("↑ Badet", () => Go(PrisonLocation.Shower)),
					("→ Vagtrummet", () => Go(PrisonLocation.GuardRoom)));
				break;
			case PrisonLocation.GuardRoom:
				HandleGuardRoom();
				break;
			case PrisonLocation.ControlRoom:
				HandleControlRoom();
				break;
			case PrisonLocation.Storage:
				HandleStorage();
				break;
			case PrisonLocation.MainEntrance:
				HandleMainEntrance();
				break;
		}
	}

	private void HandleCafeteria()
	{
		Title = "DU ER I KANTINEN";
		if (!kitchenHacked && player is IHacker hacker)
		{
			log("Køkkendøren har en elektronisk lås.");
			hacker.HackSecurity();
			kitchenHacked = true;
		}

		log(kitchenHacked
			? "Køkkendøren er hacket. Der er stadig ingen vej ud her."
			: "Kantinen er tom. Der er ingen vagter her.");

		SetChoices(
			("↓ Gangen", () => Go(PrisonLocation.Corridor)),
			("→ Celleblok B", () => Go(PrisonLocation.CellBlockB)));
	}

	private void HandleCellBlockB()
	{
		Title = "DU ER I CELLEBLOK B";
		if (!lockerPicked && player is ILockPicker lockPicker)
		{
			log("Et skab i cellen er låst.");
			Door locker = new() { Name = "skabet" };
			locker.Lock();
			lockPicker.PickLock(locker);
			lockerPicked = true;
		}

		log(lockerPicked
			? "Skabet er dirket op. De andre celler er stadig låst."
			: "De andre celler er låst. Her er der ingen vej ud.");

		SetChoices(
			("← Kantinen", () => Go(PrisonLocation.Cafeteria)),
			("↓ Gangen", () => Go(PrisonLocation.Corridor)));
	}

	private void HandleYard()
	{
		Title = "DU ER I GÅRDEN";
		if (yardCleared)
		{
			log("Gården er tom nu.");
			SetChoices(
				("↑ Gangen", () => Go(PrisonLocation.Corridor)),
				("↓ Vagtrummet", () => Go(PrisonLocation.GuardRoom)));
			return;
		}

		log("En vagt patruljerer gården!");

		if (player is ISneaky sneaky)
		{
			sneaky.Sneak();
			yardCleared = true;
			log("Du kom forbi vagten.");
			SetChoices(
				("↑ Gangen", () => Go(PrisonLocation.Corridor)),
				("↓ Vagtrummet", () => Go(PrisonLocation.GuardRoom)));
			return;
		}

		if (player is ISuperStrong)
		{
			log($"{player.Name} overvælder vagten med rå styrke.");
			yardCleared = true;
			log("Vagten er ude af spillet.");
			SetChoices(
				("↑ Gangen", () => Go(PrisonLocation.Corridor)),
				("↓ Vagtrummet", () => Go(PrisonLocation.GuardRoom)));
			return;
		}

		Title = "EN VAGT BLOKERER GÅRDEN";
		log("Du har ikke evnen til at snige dig eller overmande vagten.");
		SetChoices(
			("↑ Tilbage til gangen", () => Go(PrisonLocation.Corridor)),
			("Prøv at passere alligevel", () => GetCaught("Vagten opdager dig i gården!")));
	}

	private void HandleGuardRoom()
	{
		if (guardRoomCleared)
		{
			ShowGuardRoomChoices("Vagtrummet er passeret.");
			return;
		}

		Title = "VAGTER I VAGTRUMMET";
		log("Vagterne holder øje med vagtrummet!");

		if (player is ISneaky sneaky)
		{
			sneaky.Sneak();
			guardRoomCleared = true;
			ShowGuardRoomChoices("Du kom uset gennem vagtrummet.");
			return;
		}

		if (player is ISuperStrong strongPrisoner)
		{
			Door guardDoor = new() { Name = "vagtrummets dør" };
			strongPrisoner.BreakDoor(guardDoor);
			guardRoomCleared = true;
			ShowGuardRoomChoices("Du tvang dig gennem vagtrummet.");
			return;
		}

		if (player is ISecurityHacker securityHacker)
		{
			securityHacker.DisableCameras();
			guardRoomCleared = true;
			ShowGuardRoomChoices("Kameraerne er slået fra. Du glider gennem vagtrummet.");
			return;
		}

		log("Vagterne holder øje med rummet. Uden den rette evne er det risikabelt.");
		SetChoices(
			("↑ Tilbage til gården", () => Go(PrisonLocation.Yard)),
			("← Tilbage til sygestuen", () => Go(PrisonLocation.Medical)),
			("Prøv at passere alligevel", () => GetCaught("Vagterne fanger dig i vagtrummet!")));
	}

	private void ShowGuardRoomChoices(string description)
	{
		Title = "DU ER I VAGTRUMMET";
		log(description);
		SetChoices(
			("↑ Gården", () => Go(PrisonLocation.Yard)),
			("← Sygestuen", () => Go(PrisonLocation.Medical)),
			("→ Kontrolrummet", () => Go(PrisonLocation.ControlRoom)),
			("↓ Hovedindgangen", () => Go(PrisonLocation.MainEntrance)));
	}

	private void HandleControlRoom()
	{
		Title = "DU ER I KONTROLRUMMET";

		if (player is ISecurityHacker securityHacker)
		{
			if (!securityDisabled)
			{
				securityHacker.DisableCameras();
				securityHacker.DisableAlarm();
				alarmTriggered = false;
				securityDisabled = true;
				DisableAlarmIncident();
				log("Du har deaktiveret kameraerne og alarmsystemet.");
			}
			else
			{
				log("Sikkerhedssystemet er allerede slået fra.");
			}

			SetChoices(
				("← Vagtrummet", () => Go(PrisonLocation.GuardRoom)),
				("→ Lageret", () => Go(PrisonLocation.Storage)));
			return;
		}

		if (alarmTriggered)
		{
			log("Alarmen blæser stadig. Du bør komme væk herfra.");
			SetChoices(
				("← Vagtrummet", () => Go(PrisonLocation.GuardRoom)),
				("→ Lageret", () => Go(PrisonLocation.Storage)));
			return;
		}

		log("Et alarmpanel blinker. Uden hacking-evner bør du lade det være.");
		SetChoices(
			("← Vagtrummet", () => Go(PrisonLocation.GuardRoom)),
			("→ Lageret", () => Go(PrisonLocation.Storage)),
			("Rør ved alarmpanelet", TriggerPanelAlarm));
	}

	private void TriggerPanelAlarm()
	{
		alarmTriggered = true;
		ReportAlarmIncident();
		log("Alarmen er udløst. Vagterne bevogter nu hovedindgangen.");
		log("Alarmen blæser. Du bør komme væk herfra.");
		SetChoices(
			("← Vagtrummet", () => Go(PrisonLocation.GuardRoom)),
			("→ Lageret", () => Go(PrisonLocation.Storage)));
		Changed?.Invoke();
	}

	private void HandleStorage()
	{
		Title = "DU ER PÅ LAGERET";
		if (!cratesMoved)
		{
			if (player is IObstacleMover mover)
			{
				mover.MoveObstacle("de tunge kasser");
				cratesMoved = true;
			}
			else
			{
				Title = "KASSER SPÆRRER VEJEN";
				log("Tunge kasser blokerer døren ud til hovedindgangen.");
				log("Uden rå styrke kan du ikke komme denne vej.");
				SetChoices(("← Tilbage til kontrolrummet", () => Go(PrisonLocation.ControlRoom)));
				return;
			}
		}

		log("Kasserne er flyttet. Vejen til hovedindgangen er fri.");
		SetChoices(
			("← Kontrolrummet", () => Go(PrisonLocation.ControlRoom)),
			("↓ Hovedindgangen", () => Go(PrisonLocation.MainEntrance)));
	}

	private void HandleMainEntrance()
	{
		Title = "HOVEDINDGANGEN";
		if (alarmTriggered && player is not ISneaky && player is not ISecurityHacker)
		{
			GetCaught("Alarmen har sat vagterne i beredskab ved hovedindgangen!");
			return;
		}

		log("Du er ved hovedindgangen. Vagterne har låst den store port.");
		SetChoices(("Åbn porten", OpenMainGate));
	}

	private void OpenMainGate()
	{
		if (!escaping)
		{
			return;
		}

		Door mainGate = new() { Name = "hovedporten" };
		mainGate.Lock();
		OpenLockedDoor(mainGate);

		if (!escaping)
		{
			return;
		}

		ShowEscaped();
	}

	private void ShowEscaped()
	{
		player.Status = PrisonerStatus.Escaped;
		escaping = false;
		Title = "DU ER FLYGTET!";
		log($"{player.Name} er ude af fængslet.");
		log("Flugten lykkedes.");
		CloseEscapeIncident();
		SetChoices(("Tilbage til menuen", () => Changed?.Invoke()));
	}

	private void GetCaught(string reason)
	{
		player.Status = PrisonerStatus.Caught;
		escaping = false;
		Title = "DU ER FANGET!";
		log(reason);
		log("Flugten mislykkedes.");
		CloseEscapeIncident();
		SetChoices(("Tilbage til menuen", () => Changed?.Invoke()));
	}

	private void NotifyControlCenterOfEscape()
	{
		escapeIncident = CreateIncident("Flugtforsøg", "Celleblok A", Severity.High);
		controlCenter.ReportIncident(escapeIncident);

		alarmIncident = CreateIncident("Flugtalarm", "Kontrolcentralen", Severity.High);
		controlCenter.RaiseAlarm(alarmIncident);

		log("KONTROLCENTRALEN har udløst alarm!");
		log("Ny hændelse registreret: Flugtforsøg i Celleblok A.");
		log("Ny hændelse registreret: Flugtalarm.");
		log("Vagterne er varslet, men hovedindgangen er endnu ikke spærret.");

		try
		{
			controlCenter.CheckPrisonerAvailability(player);
		}
		catch (PrisonerUnavailableException ex)
		{
			log(ex.Message);
		}

		try
		{
			controlCenter.AssignPrisoner(escapeIncident);
		}
		catch (NoSuitablePrisonerException ex)
		{
			log(ex.Message);
		}

		ShowUnresolvedIncidents();
	}

	private void ReportAlarmIncident()
	{
		alarmIncident = CreateIncident("Alarm aktiveret", "Kontrolrum", Severity.Critical);
		controlCenter.ReportIncident(alarmIncident);
		log("KONTROLCENTRALEN");
		log("Ny hændelse registreret: Alarm aktiveret i kontrolrummet.");
		ShowUnresolvedIncidents();
	}

	private void DisableAlarmIncident()
	{
		if (alarmIncident != null && !alarmIncident.IsResolved)
		{
			alarmIncident.Resolve();
			return;
		}

		alarmIncident = CreateIncident("Alarmsystem deaktiveret", "Kontrolrum", Severity.High);
		controlCenter.ReportIncident(alarmIncident);
		alarmIncident.Resolve();
	}

	private Incident CreateIncident(string description, string location, Severity severity)
	{
		Incident incident = new()
		{
			Description = description,
			Location = location,
			Severity = severity
		};

		incident.OnResolved += LogResolvedIncident;
		incident.OnResolved += resolved =>
			log($"[LOG] {resolved.Description} i {resolved.Location} er afsluttet.");

		return incident;
	}

	private void LogResolvedIncident(Incident incident)
	{
		log($"Kontrolcentralen: Hændelsen '{incident.Description}' er markeret som løst.");
	}

	private void CloseEscapeIncident()
	{
		if (escapeIncident != null && !escapeIncident.IsResolved)
		{
			escapeIncident.Resolve();
		}

		if (alarmIncident != null && !alarmIncident.IsResolved)
		{
			alarmIncident.Resolve();
		}
	}

	private void ShowUnresolvedIncidents()
	{
		List<Incident> unresolved = controlCenter.GetUnresolvedIncidents();
		if (unresolved.Count == 0)
		{
			log("Uafklarede hændelser: Ingen.");
			return;
		}

		log("Uafklarede hændelser:");
		foreach (Incident incident in unresolved)
		{
			log($"  - {incident.Description} ({incident.Location}) [{incident.Severity}]");
		}
	}

	private void SetChoices(params (string Label, Action Act)[] items)
	{
		choices = items.ToList();
		Changed?.Invoke();
	}
}
