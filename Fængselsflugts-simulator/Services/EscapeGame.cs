using Fængselsflugts_simulator.Enums;
using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models;
using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.UI;

namespace Fængselsflugts_simulator.Services
{
	/// <summary>
	/// Styrer spillerens flugtforsøg gennem fængslet.
	/// </summary>
	internal class EscapeGame
	{
		private readonly PrisonControlCenter controlCenter;
		private Prisoner player = null!;
		private PrisonLocation currentLocation;
		private bool escaping;
		private bool alarmTriggered;
		private bool cratesMoved;
		private bool yardCleared;
		private bool guardRoomCleared;
		private bool securityDisabled;
		private Incident? escapeIncident;
		private Incident? alarmIncident;

		/// <summary>
		/// Opretter flugten med den fælles kontrolcentral.
		/// </summary>
		public EscapeGame(PrisonControlCenter controlCenter)
		{
			this.controlCenter = controlCenter;
		}

		/// <summary>
		/// Starter flugten for den valgte fange.
		/// </summary>
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
			escapeIncident = null;
			alarmIncident = null;

			EscapeFromCell();

			if (player.Status == PrisonerStatus.Caught)
			{
				return;
			}

			currentLocation = PrisonLocation.Corridor;
			escaping = true;

			while (escaping)
			{
				switch (currentLocation)
				{
					case PrisonLocation.CellBlockA:
						currentLocation = ChooseFromCellBlockA();
						break;

					case PrisonLocation.Corridor:
						currentLocation = ChooseFromCorridor();
						break;

					case PrisonLocation.Cafeteria:
						currentLocation = ChooseFromCafeteria();
						break;

					case PrisonLocation.CellBlockB:
						currentLocation = ChooseFromCellBlockB();
						break;

					case PrisonLocation.Shower:
						currentLocation = ChooseFromShower();
						break;

					case PrisonLocation.Yard:
						HandleYard();
						break;

					case PrisonLocation.Medical:
						currentLocation = ChooseFromMedical();
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
		}

		private void EscapeFromCell()
		{
			Console.Clear();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("FLUGTEN BEGYNDER\n");
			Console.ResetColor();

			Console.WriteLine("Du sidder i din celle.");
			Console.WriteLine("Foran dig er en låst celledør.");
			Console.WriteLine("Find vejen til hovedindgangen, og kom ud.\n");

			Console.WriteLine("Tryk ENTER for at forsøge at komme ud.");
			WaitForEnter();

			Console.Clear();

			Door cellDoor = new Door
			{
				Name = "celledøren"
			};

			OpenLockedDoor(cellDoor);

			if (player.Status == PrisonerStatus.Caught)
			{
				return;
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("\nDu er ude af cellen!");
			Console.ResetColor();

			NotifyControlCenterOfEscape();
			WaitToContinue();
		}

		/// <summary>
		/// Åbner en låst dør. Bruger PerformSpecialAction og fangens interfaces.
		/// </summary>
		private void OpenLockedDoor(Door door)
		{
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

		private PrisonLocation ChooseFromCellBlockA()
		{
			DirectionMenu.Show(
				PrisonLocation.CellBlockA,
				"DU ER I CELLEBLOK A - HVOR VIL DU GÅ?",
				new[] { "→ Gangen" },
				"Celledøren står åben bag dig.");

			return PrisonLocation.Corridor;
		}

		private PrisonLocation ChooseFromCorridor()
		{
			string[] options =
			{
				"← Celleblok A",
				"↑ Kantinen",
				"→ Celleblok B",
				"↓ Badet",
				"↓ Gården"
			};

			int choice = DirectionMenu.Show(
				PrisonLocation.Corridor,
				"DU ER PÅ GANGEN - HVOR VIL DU GÅ?",
				options,
				"Gangen forbinder fængslets områder. Hovedindgangen ligger i den anden ende.");

			return choice switch
			{
				0 => PrisonLocation.CellBlockA,
				1 => PrisonLocation.Cafeteria,
				2 => PrisonLocation.CellBlockB,
				3 => PrisonLocation.Shower,
				4 => PrisonLocation.Yard,
				_ => PrisonLocation.Corridor
			};
		}

		private PrisonLocation ChooseFromCafeteria()
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.Cafeteria,
				"DU ER I KANTINEN - HVOR VIL DU GÅ?",
				new[]
				{
					"↓ Gangen",
					"→ Celleblok B"
				},
				"Kantinen er tom. Der er ingen vagter her.");

			return choice switch
			{
				0 => PrisonLocation.Corridor,
				1 => PrisonLocation.CellBlockB,
				_ => PrisonLocation.Cafeteria
			};
		}

		private PrisonLocation ChooseFromCellBlockB()
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.CellBlockB,
				"DU ER I CELLEBLOK B - HVOR VIL DU GÅ?",
				new[]
				{
					"← Kantinen",
					"↓ Gangen"
				},
				"De andre celler er låst. Her er der ingen vej ud.");

			return choice switch
			{
				0 => PrisonLocation.Cafeteria,
				1 => PrisonLocation.Corridor,
				_ => PrisonLocation.CellBlockB
			};
		}

		private PrisonLocation ChooseFromShower()
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.Shower,
				"DU ER I BADDET - HVOR VIL DU GÅ?",
				new[]
				{
					"↑ Gangen",
					"↓ Sygestuen"
				},
				"Badet er stille. En bagvej fører videre mod sygestuen.");

			return choice switch
			{
				0 => PrisonLocation.Corridor,
				1 => PrisonLocation.Medical,
				_ => PrisonLocation.Shower
			};
		}

		// Gården: vagt. ISneaky og ISuperStrong kan passere.
		private void HandleYard()
		{
			if (yardCleared)
			{
				currentLocation = ChooseFromYard("Gården er tom nu.");
				return;
			}

			PrisonMap.Draw(PrisonLocation.Yard);

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("\nEn vagt patruljerer gården!");
			Console.ResetColor();
			Console.WriteLine();

			if (player is ISneaky sneaky)
			{
				sneaky.Sneak();
				WaitToContinue();
				yardCleared = true;
				currentLocation = ChooseFromYard("Du kom forbi vagten.");
				return;
			}

			if (player is ISuperStrong)
			{
				Console.WriteLine($"{player.Name} overvælder vagten med rå styrke.");
				WaitToContinue();
				yardCleared = true;
				currentLocation = ChooseFromYard("Vagten er ude af spillet.");
				return;
			}

			int choice = DirectionMenu.Show(
				PrisonLocation.Yard,
				"EN VAGT BLOKERER GÅRDEN",
				new[]
				{
					"↑ Tilbage til gangen",
					"Prøv at passere alligevel"
				},
				"Du har ikke evnen til at snige dig eller overmande vagten.");

			if (choice == 0)
			{
				currentLocation = PrisonLocation.Corridor;
			}
			else
			{
				GetCaught("Vagten opdager dig i gården!");
			}
		}

		private PrisonLocation ChooseFromYard(string description)
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.Yard,
				"DU ER I GÅRDEN - HVOR VIL DU GÅ?",
				new[]
				{
					"↑ Gangen",
					"↓ Vagtrummet"
				},
				description);

			return choice switch
			{
				0 => PrisonLocation.Corridor,
				1 => PrisonLocation.GuardRoom,
				_ => PrisonLocation.Yard
			};
		}

		private PrisonLocation ChooseFromMedical()
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.Medical,
				"DU ER PÅ SYGESTUEN - HVOR VIL DU GÅ?",
				new[]
				{
					"↑ Badet",
					"→ Vagtrummet"
				},
				"Sygestuen er tom. Herfra kan du komme ind i vagtrummet.");

			return choice switch
			{
				0 => PrisonLocation.Shower,
				1 => PrisonLocation.GuardRoom,
				_ => PrisonLocation.Medical
			};
		}

		// Vagtrummet: vagter. Sneak, styrke eller kamera-hack lader dig passere.
		private void HandleGuardRoom()
		{
			if (guardRoomCleared)
			{
				currentLocation = ChooseFromGuardRoom("Vagtrummet er passeret.");
				return;
			}

			PrisonMap.Draw(PrisonLocation.GuardRoom);

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("\nVagterne holder øje med vagtrummet!");
			Console.ResetColor();
			Console.WriteLine();

			if (player is ISneaky sneaky)
			{
				sneaky.Sneak();
				WaitToContinue();
				guardRoomCleared = true;
				currentLocation = ChooseFromGuardRoom("Du kom uset gennem vagtrummet.");
				return;
			}

			if (player is ISuperStrong strongPrisoner)
			{
				Door guardDoor = new Door { Name = "vagtrummets dør" };
				strongPrisoner.BreakDoor(guardDoor);
				WaitToContinue();
				guardRoomCleared = true;
				currentLocation = ChooseFromGuardRoom("Du tvang dig gennem vagtrummet.");
				return;
			}

			if (player is ISecurityHacker securityHacker)
			{
				securityHacker.DisableCameras();
				WaitToContinue();
				guardRoomCleared = true;
				currentLocation = ChooseFromGuardRoom("Kameraerne er slået fra. Du glider gennem vagtrummet.");
				return;
			}

			int choice = DirectionMenu.Show(
				PrisonLocation.GuardRoom,
				"VAGTER I VAGTRUMMET",
				new[]
				{
					"↑ Tilbage til gården",
					"← Tilbage til sygestuen",
					"Prøv at passere alligevel"
				},
				"Vagterne holder øje med rummet. Uden den rette evne er det risikabelt.");

			if (choice == 0)
			{
				currentLocation = PrisonLocation.Yard;
			}
			else if (choice == 1)
			{
				currentLocation = PrisonLocation.Medical;
			}
			else
			{
				GetCaught("Vagterne fanger dig i vagtrummet!");
			}
		}

		private PrisonLocation ChooseFromGuardRoom(string description)
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.GuardRoom,
				"DU ER I VAGTRUMMET - HVOR VIL DU GÅ?",
				new[]
				{
					"↑ Gården",
					"← Sygestuen",
					"→ Kontrolrummet",
					"↓ Hovedindgangen"
				},
				description);

			return choice switch
			{
				0 => PrisonLocation.Yard,
				1 => PrisonLocation.Medical,
				2 => PrisonLocation.ControlRoom,
				3 => PrisonLocation.MainEntrance,
				_ => PrisonLocation.GuardRoom
			};
		}

		// Kontrolrummet: alarm og kameraer. Kun ISecurityHacker kan slå systemet fra.
		private void HandleControlRoom()
		{
			if (player is ISecurityHacker securityHacker)
			{
				if (securityDisabled)
				{
					currentLocation = ChooseFromControlRoom(
						"Sikkerhedssystemet er allerede slået fra.");
					return;
				}

				PrisonMap.Draw(PrisonLocation.ControlRoom);
				Console.WriteLine();
				securityHacker.DisableCameras();
				securityHacker.DisableAlarm();
				alarmTriggered = false;
				securityDisabled = true;

				DisableAlarmIncident();
				WaitToContinue();

				currentLocation = ChooseFromControlRoom(
					"Du har deaktiveret kameraerne og alarmsystemet.");
				return;
			}

			if (alarmTriggered)
			{
				currentLocation = ChooseFromControlRoom(
					"Alarmen blæser stadig. Du bør komme væk herfra.");
				return;
			}

			int choice = DirectionMenu.Show(
				PrisonLocation.ControlRoom,
				"DU ER I KONTROLRUMMET - HVOR VIL DU GÅ?",
				new[]
				{
					"← Vagtrummet",
					"→ Lageret",
					"Rør ved alarmpanelet"
				},
				"Et alarmpanel blinker. Uden hacking-evner bør du lade det være.");

			if (choice == 2)
			{
				AlarmAnimation.Show();
				alarmTriggered = true;
				ReportAlarmIncident();

				Console.WriteLine();
				Console.WriteLine("Alarmen er udløst. Vagterne bevogter nu hovedindgangen.");
				WaitToContinue();

				currentLocation = ChooseFromControlRoom(
					"Alarmen blæser. Du bør komme væk herfra.");
				return;
			}

			currentLocation = choice == 0
				? PrisonLocation.GuardRoom
				: PrisonLocation.Storage;
		}

		private PrisonLocation ChooseFromControlRoom(string description)
		{
			int choice = DirectionMenu.Show(
				PrisonLocation.ControlRoom,
				"DU ER I KONTROLRUMMET - HVOR VIL DU GÅ?",
				new[]
				{
					"← Vagtrummet",
					"→ Lageret"
				},
				description);

			return choice switch
			{
				0 => PrisonLocation.GuardRoom,
				1 => PrisonLocation.Storage,
				_ => PrisonLocation.ControlRoom
			};
		}

		// Lageret: tunge kasser spærrer vejen til hovedindgangen.
		private void HandleStorage()
		{
			if (!cratesMoved)
			{
				if (player is IObstacleMover mover)
				{
					PrisonMap.Draw(PrisonLocation.Storage);
					Console.WriteLine();
					mover.MoveObstacle("de tunge kasser");
					cratesMoved = true;
					WaitToContinue();
				}
				else
				{
					DirectionMenu.Show(
						PrisonLocation.Storage,
						"KASSER SPÆRRER VEJEN",
						new[] { "← Tilbage til kontrolrummet" },
						"Tunge kasser blokerer døren ud til hovedindgangen.\nUden rå styrke kan du ikke komme denne vej.");

					currentLocation = PrisonLocation.ControlRoom;
					return;
				}
			}

			int choice = DirectionMenu.Show(
				PrisonLocation.Storage,
				"DU ER PÅ LAGERET - HVOR VIL DU GÅ?",
				new[]
				{
					"← Kontrolrummet",
					"↓ Hovedindgangen"
				},
				"Kasserne er flyttet. Vejen til hovedindgangen er fri.");

			currentLocation = choice switch
			{
				0 => PrisonLocation.ControlRoom,
				1 => PrisonLocation.MainEntrance,
				_ => PrisonLocation.Storage
			};
		}

		private void HandleMainEntrance()
		{
			if (alarmTriggered && player is not ISneaky && player is not ISecurityHacker)
			{
				GetCaught("Alarmen har sat vagterne i beredskab ved hovedindgangen!");
				return;
			}

			PrisonMap.Draw(PrisonLocation.MainEntrance);

			Console.WriteLine();
			Console.WriteLine("Du er ved hovedindgangen. Den store port er låst.");
			Console.WriteLine("\nTryk ENTER for at åbne porten.");
			WaitForEnter();

			if (!escaping)
			{
				return;
			}

			Console.Clear();

			Door mainGate = new Door
			{
				Name = "hovedporten"
			};

			OpenLockedDoor(mainGate);

			if (!escaping)
			{
				return;
			}

			WaitToContinue();
			ShowEscaped();
		}

		private void ShowEscaped()
		{
			player.Status = PrisonerStatus.Escaped;
			escaping = false;

			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("╔════════════════════════════════════════════╗");
			Console.WriteLine("║              DU ER FLYGTET!                ║");
			Console.WriteLine("╚════════════════════════════════════════════╝");
			Console.ResetColor();

			Console.WriteLine();
			Console.WriteLine($"{player.Name} er ude af fængslet.");
			Console.WriteLine("Flugten lykkedes.");
			CloseEscapeIncident();
			Console.WriteLine("\nTryk ENTER for at vende tilbage til menuen.");
			WaitForEnter();
		}

		private void GetCaught(string reason)
		{
			player.Status = PrisonerStatus.Caught;
			escaping = false;

			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("╔════════════════════════════════════════════╗");
			Console.WriteLine("║              DU ER FANGET!                 ║");
			Console.WriteLine("╚════════════════════════════════════════════╝");
			Console.ResetColor();

			Console.WriteLine();
			Console.WriteLine(reason);
			Console.WriteLine("Flugten mislykkedes.");
			CloseEscapeIncident();
			Console.WriteLine("\nTryk ENTER for at vende tilbage til menuen.");
			WaitForEnter();
		}

		/// <summary>
		/// Registrerer flugtforsøget i kontrolcentralen og håndterer egne exceptions.
		/// </summary>
		private void NotifyControlCenterOfEscape()
		{
			escapeIncident = CreateIncident(
				"Flugtforsøg",
				"Celleblok A",
				Severity.High);

			controlCenter.ReportIncident(escapeIncident);

			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("KONTROLCENTRALEN");
			Console.ResetColor();
			Console.WriteLine("Ny hændelse registreret: Flugtforsøg i Celleblok A.");

			try
			{
				controlCenter.CheckPrisonerAvailability(player);
			}
			catch (PrisonerUnavailableException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}

			try
			{
				controlCenter.AssignPrisoner(escapeIncident);
			}
			catch (NoSuitablePrisonerException ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ResetColor();
			}

			ShowUnresolvedIncidents();
		}

		private void ReportAlarmIncident()
		{
			alarmIncident = CreateIncident(
				"Alarm aktiveret",
				"Kontrolrum",
				Severity.Critical);

			controlCenter.ReportIncident(alarmIncident);

			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine("KONTROLCENTRALEN");
			Console.ResetColor();
			Console.WriteLine("Ny hændelse registreret: Alarm aktiveret i kontrolrummet.");

			ShowUnresolvedIncidents();
		}

		private void DisableAlarmIncident()
		{
			if (alarmIncident != null && !alarmIncident.IsResolved)
			{
				alarmIncident.Resolve();
				return;
			}

			alarmIncident = CreateIncident(
				"Alarmsystem deaktiveret",
				"Kontrolrum",
				Severity.High);

			controlCenter.ReportIncident(alarmIncident);
			alarmIncident.Resolve();
		}

		/// <summary>
		/// Opretter en hændelse og tilknytter både en named method og en lambda som callback.
		/// </summary>
		private Incident CreateIncident(string description, string location, Severity severity)
		{
			Incident incident = new Incident
			{
				Description = description,
				Location = location,
				Severity = severity
			};

			// Named method + lambda: begge kaldes, når hændelsen bliver løst.
			incident.OnResolved += LogResolvedIncident;
			incident.OnResolved += resolved =>
				Console.WriteLine(
					$"[LOG] {resolved.Description} i {resolved.Location} er afsluttet.");

			return incident;
		}

		/// <summary>
		/// Named callback: kaldes, når en hændelse bliver markeret som løst.
		/// </summary>
		private void LogResolvedIncident(Incident incident)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(
				$"Kontrolcentralen: Hændelsen '{incident.Description}' er markeret som løst.");
			Console.ResetColor();
		}

		private void CloseEscapeIncident()
		{
			if (escapeIncident != null && !escapeIncident.IsResolved)
			{
				escapeIncident.Resolve();
			}
		}

		private void ShowUnresolvedIncidents()
		{
			Console.WriteLine("Uafklarede hændelser:");

			List<Incident> unresolved = controlCenter.GetUnresolvedIncidents();
			if (unresolved.Count == 0)
			{
				Console.WriteLine("  Ingen.");
				return;
			}

			foreach (Incident incident in unresolved)
			{
				Console.WriteLine(
					$"  - {incident.Description} ({incident.Location}) [{incident.Severity}]");
			}
		}

		private static void WaitToContinue()
		{
			Console.WriteLine("\nTryk ENTER for at fortsætte.");
			WaitForEnter();
		}

		private static void WaitForEnter()
		{
			while (Console.ReadKey(true).Key != ConsoleKey.Enter)
			{
			}
		}
	}
}
