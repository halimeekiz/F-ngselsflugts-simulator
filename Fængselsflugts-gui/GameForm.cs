using Fængselsflugts_simulator.Enums;
using Fængselsflugts_simulator.Exceptions;
using Fængselsflugts_simulator.Interfaces;
using Fængselsflugts_simulator.Models.Incidents;
using Fængselsflugts_simulator.Models.Prisoners;
using Fængselsflugts_simulator.Services;
using Fængselsflugts_simulator.Strategies;

namespace Fængselsflugts_simulator.Gui;

internal sealed class GameForm : Form
{
	private static readonly Color Bg = Color.FromArgb(12, 11, 10);
	private static readonly Color PanelBg = Color.FromArgb(22, 19, 17);
	private static readonly Color Gold = Color.FromArgb(214, 162, 64);
	private static readonly Color Cream = Color.FromArgb(232, 218, 196);
	private static readonly Color Mute = Color.FromArgb(156, 140, 122);
	private static readonly Color Danger = Color.FromArgb(176, 48, 42);

	private readonly PrisonMapPanel map = new();
	private readonly RoomViewPanel roomView = new();
	private readonly RichTextBox logBox = new();
	private readonly FlowLayoutPanel actions = new();
	private readonly Label titleLabel = new();
	private readonly Label playerLabel = new();
	private readonly Label statusLabel = new();
	private readonly Label sceneLabel = new();
	private readonly Panel hud = new();
	private readonly Panel intro = new();
	private readonly System.Windows.Forms.Timer pulseTimer = new();

	private Prisoner? player;
	private PrisonControlCenter? controlCenter;
	private GuiEscapeGame? escape;
	private float pulse;
	private bool pulseUp = true;
	private Screen screen = Screen.Intro;

	private enum Screen
	{
		Intro,
		Select,
		Hub,
		Play,
		Info
	}

	public GameForm()
	{
		Text = "Fængselsflugts-simulator";
		StartPosition = FormStartPosition.CenterScreen;
		MinimumSize = new Size(1180, 780);
		Size = new Size(1360, 900);
		BackColor = Bg;
		ForeColor = Cream;
		Font = new Font("Segoe UI", 10f);
		DoubleBuffered = true;
		KeyPreview = true;

		BuildHud();
		BuildIntro();
		Controls.Add(hud);
		Controls.Add(intro);

		pulseTimer.Interval = 40;
		pulseTimer.Tick += (_, _) =>
		{
			pulse += pulseUp ? 0.04f : -0.04f;
			if (pulse >= 1f) { pulse = 1f; pulseUp = false; }
			if (pulse <= 0f) { pulse = 0f; pulseUp = true; }
			UpdateViews(
				escape?.Location ?? PrisonLocation.CellBlockA,
				escape?.AlarmOn == true);
		};
		pulseTimer.Start();

		Console.SetOut(new LogTextWriter(AppendLogSafe));
		ShowIntro();
	}

	private void BuildHud()
	{
		hud.Dock = DockStyle.Fill;
		hud.BackColor = Bg;
		hud.Visible = false;
		hud.Padding = new Padding(16);

		Panel top = new()
		{
			Dock = DockStyle.Top,
			Height = 72,
			BackColor = PanelBg
		};

		titleLabel.AutoSize = false;
		titleLabel.Dock = DockStyle.Left;
		titleLabel.Width = 520;
		titleLabel.Font = new Font("Segoe UI", 18f, FontStyle.Bold);
		titleLabel.ForeColor = Gold;
		titleLabel.TextAlign = ContentAlignment.MiddleLeft;
		titleLabel.Padding = new Padding(16, 0, 0, 0);
		titleLabel.Text = "FÆNGSELSFLUGTS-SIMULATOR";

		playerLabel.AutoSize = false;
		playerLabel.Dock = DockStyle.Fill;
		playerLabel.TextAlign = ContentAlignment.MiddleRight;
		playerLabel.ForeColor = Cream;
		playerLabel.Padding = new Padding(0, 0, 16, 0);

		statusLabel.AutoSize = false;
		statusLabel.Dock = DockStyle.Right;
		statusLabel.Width = 180;
		statusLabel.TextAlign = ContentAlignment.MiddleRight;
		statusLabel.ForeColor = Gold;
		statusLabel.Padding = new Padding(0, 0, 18, 0);

		top.Controls.Add(playerLabel);
		top.Controls.Add(statusLabel);
		top.Controls.Add(titleLabel);

		actions.Dock = DockStyle.Bottom;
		actions.Height = 92;
		actions.BackColor = Color.FromArgb(18, 16, 15);
		actions.Padding = new Padding(12, 14, 12, 12);
		actions.WrapContents = true;

		Panel right = new()
		{
			Dock = DockStyle.Right,
			Width = 380,
			BackColor = PanelBg,
			Padding = new Padding(16)
		};

		sceneLabel.Dock = DockStyle.Top;
		sceneLabel.Height = 48;
		sceneLabel.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
		sceneLabel.ForeColor = Gold;
		sceneLabel.Text = "MISSION";

		logBox.Dock = DockStyle.Fill;
		logBox.ReadOnly = true;
		logBox.BorderStyle = BorderStyle.None;
		logBox.BackColor = Color.FromArgb(17, 15, 14);
		logBox.ForeColor = Cream;
		logBox.Font = new Font("Segoe UI", 10.5f);
		logBox.ScrollBars = RichTextBoxScrollBars.Vertical;

		right.Controls.Add(logBox);
		right.Controls.Add(sceneLabel);

		map.Dock = DockStyle.Bottom;
		map.Height = 230;

		roomView.Dock = DockStyle.Fill;

		Panel mapHolder = new()
		{
			Dock = DockStyle.Fill,
			Padding = new Padding(12, 12, 8, 8),
			BackColor = Bg
		};
		mapHolder.Controls.Add(roomView);
		mapHolder.Controls.Add(map);

		hud.Controls.Add(mapHolder);
		hud.Controls.Add(right);
		hud.Controls.Add(actions);
		hud.Controls.Add(top);
	}

	private void BuildIntro()
	{
		intro.Dock = DockStyle.Fill;
		intro.BackColor = Bg;

		Label brand = new()
		{
			Text = "FÆNGSELSFLUGTS-SIMULATOR",
			Font = new Font("Segoe UI", 28f, FontStyle.Bold),
			ForeColor = Gold,
			AutoSize = false,
			Dock = DockStyle.Top,
			Height = 90,
			TextAlign = ContentAlignment.BottomCenter
		};

		Label story = new()
		{
			Text = "Du er fanget i et topsikret fængsel.\nFind en vej ud uden at blive opdaget.\n\nLåste døre  ·  vagter  ·  kameraer  ·  alarm",
			Font = new Font("Segoe UI", 13f),
			ForeColor = Cream,
			AutoSize = false,
			Dock = DockStyle.Fill,
			TextAlign = ContentAlignment.MiddleCenter
		};

		FlowLayoutPanel bottom = new()
		{
			Dock = DockStyle.Bottom,
			Height = 90,
			FlowDirection = FlowDirection.LeftToRight,
			Padding = new Padding(0, 10, 0, 20),
			BackColor = Bg
		};

		Button start = MakeButton("Start spillet", 220);
		start.Click += (_, _) => ShowSelect();
		bottom.Controls.Add(start);
		CenterFlow(bottom);

		intro.Controls.Add(story);
		intro.Controls.Add(bottom);
		intro.Controls.Add(brand);
		intro.Resize += (_, _) => CenterFlow(bottom);
	}

	private void ShowIntro()
	{
		screen = Screen.Intro;
		hud.Visible = false;
		intro.Visible = true;
		intro.BringToFront();
	}

	private void ShowSelect()
	{
		screen = Screen.Select;
		intro.Visible = false;
		hud.Visible = true;
		hud.BringToFront();
		ClearLog();
		sceneLabel.Text = "VÆLG DIN FANGE";
		titleLabel.Text = "FÆNGSELSFLUGTS-SIMULATOR";
		playerLabel.Text = "";
		statusLabel.Text = "";
		AppendLog("Din fangetype bestemmer, hvordan du kommer ud.");
		AppendLog("Escape Artist dirker og sniger. Hacker slår systemer fra. Strong bryder døre og flytter kasser.");
		SetButtons(
			("Escape Artist", () => ChoosePlayer(new EscapeArtist(1, "Escape Artist"))),
			("Hacker", () => ChoosePlayer(new HackerPrisoner(2, "Hacker"))),
			("Strong Prisoner", () => ChoosePlayer(new StrongPrisoner(3, "Strong"))));
		UpdateViews(PrisonLocation.CellBlockA, false);
	}

	private void ChoosePlayer(Prisoner chosen)
	{
		player = chosen;
		controlCenter = new PrisonControlCenter(new FirstAvailablePrisonerStrategy());
		RegisterPrisoners();
		ShowHub();
	}

	private void RegisterPrisoners()
	{
		if (player == null || controlCenter == null)
		{
			return;
		}

		controlCenter.RegisterPrisoner(player);

		Prisoner[] others =
		{
			new EscapeArtist(10, "Luna"),
			new HackerPrisoner(11, "Omar"),
			new StrongPrisoner(12, "Bo")
		};

		foreach (Prisoner other in others)
		{
			if (other.GetType() == player.GetType())
			{
				continue;
			}

			other.IsAvailable = false;
			other.Status = PrisonerStatus.InCell;
			controlCenter.RegisterPrisoner(other);
		}
	}

	private void ShowHub()
	{
		if (player == null)
		{
			return;
		}

		screen = Screen.Hub;
		escape = null;
		sceneLabel.Text = "HOVEDMENU";
		titleLabel.Text = "FÆNGSELSFLUGTS-SIMULATOR";
		RefreshPlayerBar();
		ClearLog();
		AppendLog($"Velkommen, {player.Name}.");
		AppendLog("Start flugten, når du er klar. Målet er hovedindgangen.");
		SetButtons(
			("Start flugten", StartEscape),
			("Dine evner", ShowAbilities),
			("Hændelser", ShowIncidents),
			("Tildelingsstrategier", ShowStrategies),
			("Afslut", Close));
		UpdateViews(PrisonLocation.CellBlockA, false);
	}

	private void StartEscape()
	{
		if (player == null || controlCenter == null)
		{
			return;
		}

		screen = Screen.Play;
		ClearLog();
		escape = new GuiEscapeGame(controlCenter, AppendLogSafe);
		escape.Changed += OnEscapeChanged;
		escape.Start(player);
		OnEscapeChanged();
	}

	private void OnEscapeChanged()
	{
		if (IsDisposed)
		{
			return;
		}

		if (InvokeRequired)
		{
			BeginInvoke(OnEscapeChanged);
			return;
		}

		if (escape == null)
		{
			return;
		}

		sceneLabel.Text = escape.Title;
		RefreshPlayerBar();
		UpdateViews(escape.Location, escape.AlarmOn);

		if (escape.Finished)
		{
			var labels = escape.ChoiceLabels;
			SetButtons((labels.Count > 0 ? labels[0] : "Tilbage til menuen", ShowHub));
			return;
		}

		SetButtons(escape.ChoiceLabels.Select((label, i) => (label, (Action)(() => escape.Pick(i)))).ToArray());
	}

	private void ShowAbilities()
	{
		if (player == null)
		{
			return;
		}

		screen = Screen.Info;
		sceneLabel.Text = "DINE EVNER";
		ClearLog();
		AppendLog($"Fange: {player.Name}");
		AppendLog($"Power: {player.PowerLevel}");
		AppendLog($"Status: {StatusText(player.Status)}");
		AppendLog("");
		if (player is ILockPicker) AppendLog("Åbn eller tving låste døre op");
		if (player is IHacker) AppendLog("Hack sikkerhedssystemer");
		if (player is ISneaky) AppendLog("Snig dig forbi vagter");
		if (player is ISecurityHacker)
		{
			AppendLog("Deaktiver kameraer");
			AppendLog("Deaktiver alarm");
		}
		if (player is ISuperStrong) AppendLog("Bryd døre med rå styrke");
		if (player is IObstacleMover) AppendLog("Flyt tunge forhindringer");
		SetButtons(("Tilbage", ShowHub));
	}

	private void ShowIncidents()
	{
		if (controlCenter == null)
		{
			return;
		}

		screen = Screen.Info;
		sceneLabel.Text = "KONTROLCENTRALEN";
		ClearLog();
		AppendLog("HÆNDELSER:");
		controlCenter.ShowIncidents();

		AppendLog("");
		AppendLog("UAFKLAREDE HÆNDELSER:");
		var unresolved = controlCenter.GetUnresolvedIncidents();
		if (unresolved.Count == 0)
		{
			AppendLog("Ingen.");
		}
		else
		{
			foreach (var incident in unresolved)
			{
				AppendLog($"- {incident.Description} ({incident.Location}) [{incident.Severity}]");
			}
		}

		AppendLog("");
		AppendLog("LEDIGE FANGER:");
		var available = controlCenter.GetAvailablePrisoners();
		if (available.Count == 0)
		{
			AppendLog("Ingen.");
		}
		else
		{
			foreach (var prisoner in available)
			{
				AppendLog($"- {prisoner.Name} (ID {prisoner.Id})");
			}
		}

		AppendLog("");
		AppendLog("FØRSTE LEDIGE FANGE:");
		try
		{
			Prisoner first = controlCenter.GetFirstAvailablePrisoner();
			AppendLog($"{first.Name} (ID {first.Id})");
		}
		catch (NoSuitablePrisonerException ex)
		{
			AppendLog(ex.Message);
		}

		SetButtons(("Tilbage", ShowHub));
	}

	private void ShowStrategies()
	{
		screen = Screen.Info;
		sceneLabel.Text = "TILDELINGSSTRATEGIER";
		ClearLog();
		AppendLog("Samme tre ledige fanger og samme hændelse.");
		AppendLog("Kun strategien i constructoren skifter.");
		AppendLog("PrisonControlCenter er uændret.");
		AppendLog("");
		AppendLog("Luna  (Escape Artist)  Power 70");
		AppendLog("Omar  (Hacker)         Power 60");
		AppendLog("Bo    (Strong)         Power 90");
		AppendLog("");

		PrisonControlCenter firstCenter = new(new FirstAvailablePrisonerStrategy());
		PrisonControlCenter powerCenter = new(new HighestPowerPrisonerStrategy());
		RegisterDemo(firstCenter);
		RegisterDemo(powerCenter);

		Incident incident = new()
		{
			Description = "Flugtforsøg i gården",
			Location = "Gård",
			Severity = Severity.High
		};

		Prisoner firstChoice = firstCenter.AssignPrisoner(incident);
		Prisoner powerChoice = powerCenter.AssignPrisoner(incident);
		AppendLog($"Første ledige:  {firstChoice.Name} (Power {firstChoice.PowerLevel})");
		AppendLog($"Højest power:   {powerChoice.Name} (Power {powerChoice.PowerLevel})");
		SetButtons(("Tilbage", ShowHub));
	}

	private static void RegisterDemo(PrisonControlCenter center)
	{
		center.RegisterPrisoner(new EscapeArtist(10, "Luna"));
		center.RegisterPrisoner(new HackerPrisoner(11, "Omar"));
		center.RegisterPrisoner(new StrongPrisoner(12, "Bo"));
	}

	private void UpdateViews(PrisonLocation location, bool alarmOn)
	{
		map.SetState(location, alarmOn, pulse);
		roomView.SetState(location, alarmOn, pulse);
	}

	private void RefreshPlayerBar()
	{
		if (player == null)
		{
			return;
		}

		playerLabel.Text = $"{player.Name}   ·   Power {player.PowerLevel}";
		statusLabel.Text = StatusText(player.Status);
		statusLabel.ForeColor = player.Status switch
		{
			PrisonerStatus.Escaped => Color.FromArgb(110, 186, 92),
			PrisonerStatus.Caught => Danger,
			PrisonerStatus.Escaping => Gold,
			_ => Mute
		};
	}

	private static string StatusText(PrisonerStatus status) => status switch
	{
		PrisonerStatus.InCell => "I cellen",
		PrisonerStatus.Escaping => "På flugt",
		PrisonerStatus.Escaped => "Flygtet",
		PrisonerStatus.Caught => "Fanget",
		_ => status.ToString()
	};

	private void SetButtons(params (string Text, Action Click)[] items)
	{
		actions.SuspendLayout();
		actions.Controls.Clear();
		foreach (var item in items)
		{
			Button button = MakeButton(item.Text, Math.Max(150, 18 + item.Text.Length * 9));
			Action click = item.Click;
			button.Click += (_, _) => click();
			actions.Controls.Add(button);
		}
		actions.ResumeLayout();
	}

	private Button MakeButton(string text, int width)
	{
		Button button = new()
		{
			Text = text,
			Width = width,
			Height = 42,
			Margin = new Padding(6, 4, 6, 4),
			FlatStyle = FlatStyle.Flat,
			BackColor = Color.FromArgb(36, 30, 26),
			ForeColor = Cream,
			Font = new Font("Segoe UI", 10f, FontStyle.Bold),
			Cursor = Cursors.Hand
		};
		button.FlatAppearance.BorderColor = Gold;
		button.FlatAppearance.BorderSize = 1;
		button.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 52, 28);
		return button;
	}

	private void CenterFlow(FlowLayoutPanel panel)
	{
		int total = 0;
		foreach (Control c in panel.Controls)
		{
			total += c.Width + c.Margin.Horizontal;
		}

		int left = Math.Max(0, (panel.ClientSize.Width - total) / 2);
		panel.Padding = new Padding(left, panel.Padding.Top, 0, panel.Padding.Bottom);
	}

	private void ClearLog()
	{
		logBox.Clear();
	}

	private void AppendLogSafe(string line)
	{
		if (IsDisposed)
		{
			return;
		}

		if (InvokeRequired)
		{
			BeginInvoke(() => AppendLog(line));
			return;
		}

		AppendLog(line);
	}

	private void AppendLog(string line)
	{
		if (string.IsNullOrWhiteSpace(line) && logBox.TextLength == 0)
		{
			return;
		}

		if (logBox.TextLength > 0)
		{
			logBox.AppendText(Environment.NewLine);
		}

		Color color = Cream;
		if (line.Contains("ALARM", StringComparison.OrdinalIgnoreCase) ||
			line.Contains("Fanget", StringComparison.OrdinalIgnoreCase) ||
			line.Contains("optaget", StringComparison.OrdinalIgnoreCase))
		{
			color = Color.FromArgb(232, 120, 96);
		}
		else if (line.Contains("Flygtet", StringComparison.OrdinalIgnoreCase) ||
			line.Contains("lykkedes", StringComparison.OrdinalIgnoreCase) ||
			line.Contains("løst", StringComparison.OrdinalIgnoreCase))
		{
			color = Color.FromArgb(140, 196, 110);
		}
		else if (line.StartsWith("KONTROL", StringComparison.Ordinal) ||
			line.StartsWith("[LOG]", StringComparison.Ordinal))
		{
			color = Gold;
		}

		logBox.SelectionStart = logBox.TextLength;
		logBox.SelectionColor = color;
		logBox.AppendText(line);
		logBox.SelectionStart = logBox.TextLength;
		logBox.ScrollToCaret();
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Escape && (screen == Screen.Info || screen == Screen.Select))
		{
			if (screen == Screen.Select)
			{
				ShowIntro();
			}
			else
			{
				ShowHub();
			}

			e.Handled = true;
		}

		base.OnKeyDown(e);
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		pulseTimer.Stop();
		base.OnFormClosed(e);
	}
}
