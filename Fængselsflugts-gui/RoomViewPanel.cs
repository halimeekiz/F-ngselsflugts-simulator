using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Gui;

internal sealed class RoomViewPanel : Panel
{
	private PrisonLocation location = PrisonLocation.CellBlockA;
	private bool alarmOn;
	private float pulse;

	public RoomViewPanel()
	{
		DoubleBuffered = true;
		BackColor = Color.Black;
		ResizeRedraw = true;
	}

	public void SetState(PrisonLocation location, bool alarmOn, float pulse)
	{
		this.location = location;
		this.alarmOn = alarmOn;
		this.pulse = pulse;
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics g = e.Graphics;
		g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		g.Clear(Color.FromArgb(8, 7, 6));

		int w = Width;
		int h = Height;
		if (w < 40 || h < 40)
		{
			return;
		}

		float bob = pulse * 4f;
		int insetX = (int)(w * 0.27f);
		int insetY = (int)(h * 0.20f + bob);

		Rectangle back = new(
			insetX,
			insetY,
			Math.Max(20, w - insetX * 2),
			Math.Max(20, h - insetY * 2 - (int)(h * 0.06f)));

		Point tl = new(0, 0);
		Point tr = new(w, 0);
		Point br = new(w, h);
		Point bl = new(0, h);
		Point btl = new(back.Left, back.Top);
		Point btr = new(back.Right, back.Top);
		Point bbr = new(back.Right, back.Bottom);
		Point bbl = new(back.Left, back.Bottom);

		Color ceiling = Theme.Ceiling;
		Color floor = Theme.Floor;
		Color left = Theme.LeftWall;
		Color right = Theme.RightWall;
		Color far = Theme.BackWall;
		ApplyRoomColors(ref ceiling, ref floor, ref left, ref right, ref far);

		FillQuad(g, tl, tr, btr, btl, ceiling);
		FillQuad(g, bl, bbl, bbr, br, floor);
		FillQuad(g, tl, btl, bbl, bl, left);
		FillQuad(g, tr, br, bbr, btr, right);
		using (SolidBrush backBrush = new(far))
		{
			g.FillRectangle(backBrush, back);
		}

		DrawPerspectiveLines(g, back, w, h);
		DrawRoomDetails(g, back, w, h);
		DrawVignette(g, w, h);

		if (alarmOn)
		{
			int alpha = 40 + (int)(70 * pulse);
			using SolidBrush fog = new(Color.FromArgb(alpha, 160, 18, 12));
			g.FillRectangle(fog, ClientRectangle);
			DrawAlarmLamp(g, back);
		}
	}

	private void ApplyRoomColors(
		ref Color ceiling,
		ref Color floor,
		ref Color left,
		ref Color right,
		ref Color far)
	{
		switch (location)
		{
			case PrisonLocation.Yard:
				ceiling = Color.FromArgb(42, 58, 72);
				floor = Color.FromArgb(58, 62, 48);
				left = Color.FromArgb(72, 78, 62);
				right = Color.FromArgb(58, 64, 50);
				far = Color.FromArgb(70, 92, 78);
				break;
			case PrisonLocation.Shower:
				ceiling = Color.FromArgb(48, 56, 62);
				floor = Color.FromArgb(36, 48, 54);
				left = Color.FromArgb(58, 70, 78);
				right = Color.FromArgb(46, 58, 66);
				far = Color.FromArgb(62, 76, 84);
				break;
			case PrisonLocation.Medical:
				ceiling = Color.FromArgb(58, 58, 54);
				floor = Color.FromArgb(48, 50, 46);
				left = Color.FromArgb(86, 84, 76);
				right = Color.FromArgb(72, 70, 64);
				far = Color.FromArgb(92, 90, 82);
				break;
			case PrisonLocation.Cafeteria:
				ceiling = Color.FromArgb(44, 36, 28);
				floor = Color.FromArgb(52, 40, 28);
				break;
			case PrisonLocation.MainEntrance:
				far = Color.FromArgb(18, 22, 16);
				break;
			case PrisonLocation.ControlRoom:
				far = Color.FromArgb(18, 28, 24);
				break;
		}
	}

	private void DrawPerspectiveLines(Graphics g, Rectangle back, int w, int h)
	{
		using Pen line = new(Color.FromArgb(40, 255, 220, 160), 1f);
		int cx = back.Left + back.Width / 2;
		int cy = back.Top + back.Height / 2;
		for (int i = 1; i <= 6; i++)
		{
			float t = i / 7f;
			int y = (int)(back.Bottom + (h - back.Bottom) * t);
			int x1 = (int)(back.Left * (1 - t));
			int x2 = (int)(back.Right + (w - back.Right) * t);
			g.DrawLine(line, x1, y, x2, y);
		}

		g.DrawLine(line, 0, h, cx, cy);
		g.DrawLine(line, w, h, cx, cy);
	}

	private void DrawRoomDetails(Graphics g, Rectangle back, int w, int h)
	{
		switch (location)
		{
			case PrisonLocation.CellBlockA:
			case PrisonLocation.CellBlockB:
				DrawBars(g, back);
				DrawBunk(g, back);
				break;
			case PrisonLocation.Cafeteria:
				DrawTables(g, back, w, h);
				break;
			case PrisonLocation.Corridor:
				DrawDeepCorridor(g, back);
				break;
			case PrisonLocation.Shower:
				DrawShowers(g, back);
				break;
			case PrisonLocation.Yard:
				DrawYard(g, back);
				break;
			case PrisonLocation.Medical:
				DrawCross(g, back);
				DrawBed(g, back, w, h);
				break;
			case PrisonLocation.GuardRoom:
				DrawDesk(g, back, w, h);
				DrawCamera(g, back);
				break;
			case PrisonLocation.ControlRoom:
				DrawScreens(g, back);
				DrawAlarmLamp(g, back);
				break;
			case PrisonLocation.Storage:
				DrawCrates(g, back, w, h);
				break;
			case PrisonLocation.MainEntrance:
				DrawGate(g, back);
				break;
		}

		if (location is not PrisonLocation.MainEntrance and not PrisonLocation.Yard)
		{
			DrawDoor(g, back, w, h);
		}
	}

	private static void DrawBars(Graphics g, Rectangle back)
	{
		using Pen bar = new(Color.FromArgb(120, 110, 95), 4f);
		int step = Math.Max(10, back.Width / 9);
		for (int x = back.Left + 8; x < back.Right - 6; x += step)
		{
			g.DrawLine(bar, x, back.Top + 6, x, back.Bottom - 6);
		}

		g.DrawLine(bar, back.Left + 6, back.Top + back.Height / 3, back.Right - 6, back.Top + back.Height / 3);
	}

	private static void DrawBunk(Graphics g, Rectangle back)
	{
		Rectangle bunk = new(back.Left + 12, back.Bottom - back.Height / 4, back.Width / 3, back.Height / 5);
		using SolidBrush mattress = new(Color.FromArgb(72, 52, 38));
		g.FillRectangle(mattress, bunk);
		using Pen edge = new(Color.FromArgb(30, 22, 16), 2f);
		g.DrawRectangle(edge, bunk);
	}

	private static void DrawTables(Graphics g, Rectangle back, int w, int h)
	{
		DrawIsoBox(g, w * 0.22f, h * 0.72f, 90, 36, 18, Color.FromArgb(86, 58, 32));
		DrawIsoBox(g, w * 0.52f, h * 0.78f, 110, 40, 18, Color.FromArgb(78, 50, 28));
	}

	private static void DrawDeepCorridor(Graphics g, Rectangle back)
	{
		Rectangle inner = new(
			back.Left + back.Width / 4,
			back.Top + back.Height / 5,
			back.Width / 2,
			back.Height * 3 / 5);
		using SolidBrush dark = new(Color.FromArgb(12, 10, 9));
		g.FillRectangle(dark, inner);
		using Pen lamp = new(Color.FromArgb(180, 160, 90), 3f);
		g.DrawLine(lamp, inner.Left + inner.Width / 2, back.Top, inner.Left + inner.Width / 2, inner.Top);
		using SolidBrush glow = new(Color.FromArgb(70, 255, 210, 110));
		g.FillEllipse(glow, inner.Left + inner.Width / 2 - 10, inner.Top - 8, 20, 12);
	}

	private static void DrawShowers(Graphics g, Rectangle back)
	{
		using Pen pipe = new(Color.FromArgb(140, 150, 155), 3f);
		for (int i = 0; i < 3; i++)
		{
			int x = back.Left + 20 + i * (back.Width / 4);
			g.DrawLine(pipe, x, back.Top + 8, x, back.Top + back.Height / 3);
			g.FillEllipse(Brushes.Silver, x - 8, back.Top + back.Height / 3, 16, 8);
			using SolidBrush steam = new(Color.FromArgb(35, 200, 220, 230));
			g.FillEllipse(steam, x - 14, back.Top + back.Height / 3 + 8, 28, 40);
		}
	}

	private static void DrawYard(Graphics g, Rectangle back)
	{
		using SolidBrush sky = new(Color.FromArgb(54, 78, 98));
		g.FillRectangle(sky, back.Left, back.Top, back.Width, back.Height / 2);
		using Pen fence = new(Color.FromArgb(90, 90, 80), 3f);
		int y = back.Top + back.Height / 2;
		g.DrawLine(fence, back.Left, y, back.Right, y);
		for (int x = back.Left + 8; x < back.Right; x += 14)
		{
			g.DrawLine(fence, x, y, x, back.Bottom - 8);
		}
	}

	private static void DrawCross(Graphics g, Rectangle back)
	{
		using SolidBrush red = new(Color.FromArgb(170, 40, 36));
		int cx = back.Left + back.Width / 2;
		int cy = back.Top + 28;
		g.FillRectangle(red, cx - 6, cy - 18, 12, 36);
		g.FillRectangle(red, cx - 16, cy - 6, 32, 12);
	}

	private static void DrawBed(Graphics g, Rectangle back, int w, int h)
	{
		DrawIsoBox(g, w * 0.58f, h * 0.74f, 100, 38, 16, Color.FromArgb(210, 210, 200));
	}

	private static void DrawDesk(Graphics g, Rectangle back, int w, int h)
	{
		DrawIsoBox(g, w * 0.36f, h * 0.76f, 120, 40, 22, Color.FromArgb(48, 40, 34));
	}

	private static void DrawCamera(Graphics g, Rectangle back)
	{
		using SolidBrush cam = new(Color.FromArgb(30, 30, 28));
		g.FillEllipse(cam, back.Right - 36, back.Top + 10, 22, 14);
		using SolidBrush lens = new(Color.FromArgb(80, 180, 80));
		g.FillEllipse(lens, back.Right - 28, back.Top + 14, 6, 6);
	}

	private void DrawScreens(Graphics g, Rectangle back)
	{
		int glow = 70 + (int)(50 * pulse);
		using SolidBrush screen = new(Color.FromArgb(glow, 40, 180, 110));
		Rectangle a = new(back.Left + 16, back.Top + 18, back.Width / 3, back.Height / 3);
		Rectangle b = new(back.Left + back.Width / 2, back.Top + 18, back.Width / 3, back.Height / 3);
		g.FillRectangle(screen, a);
		g.FillRectangle(screen, b);
		using Pen frame = new(Color.FromArgb(20, 20, 18), 3f);
		g.DrawRectangle(frame, a);
		g.DrawRectangle(frame, b);
	}

	private static void DrawCrates(Graphics g, Rectangle back, int w, int h)
	{
		DrawIsoBox(g, w * 0.20f, h * 0.76f, 70, 44, 36, Color.FromArgb(118, 78, 40));
		DrawIsoBox(g, w * 0.38f, h * 0.80f, 80, 48, 42, Color.FromArgb(102, 68, 34));
		DrawIsoBox(g, w * 0.62f, h * 0.74f, 64, 40, 30, Color.FromArgb(128, 84, 42));
	}

	private static void DrawGate(Graphics g, Rectangle back)
	{
		using SolidBrush gate = new(Color.FromArgb(28, 24, 20));
		Rectangle door = new(back.Left + back.Width / 5, back.Top + 10, back.Width * 3 / 5, back.Height - 16);
		g.FillRectangle(gate, door);
		using Pen bar = new(Color.FromArgb(90, 80, 70), 5f);
		g.DrawRectangle(bar, door);
		g.DrawLine(bar, door.Left + door.Width / 2, door.Top, door.Left + door.Width / 2, door.Bottom);
		using SolidBrush light = new(Color.FromArgb(40, 255, 200, 80));
		g.FillEllipse(light, door.Left + 12, door.Top + 18, 16, 16);
	}

	private void DrawAlarmLamp(Graphics g, Rectangle back)
	{
		int alpha = 90 + (int)(120 * pulse);
		using SolidBrush lamp = new(Color.FromArgb(alpha, 220, 40, 30));
		g.FillEllipse(lamp, back.Left + back.Width / 2 - 16, back.Top + 8, 32, 18);
	}

	private static void DrawDoor(Graphics g, Rectangle back, int w, int h)
	{
		int doorW = back.Width / 4;
		int doorH = back.Height * 2 / 3;
		Rectangle door = new(back.Left + back.Width / 2 - doorW / 2, back.Bottom - doorH, doorW, doorH);
		using SolidBrush hole = new(Color.FromArgb(16, 12, 10));
		g.FillRectangle(hole, door);
		using Pen frame = new(Color.FromArgb(70, 58, 44), 3f);
		g.DrawRectangle(frame, door);
	}

	private static void DrawIsoBox(Graphics g, float x, float y, float width, float depth, float height, Color top)
	{
		Color left = Darken(top, 0.72f);
		Color right = Darken(top, 0.55f);
		PointF t1 = new(x, y - height);
		PointF t2 = new(x + width, y - height);
		PointF t3 = new(x + width + depth * 0.5f, y - height - depth * 0.35f);
		PointF t4 = new(x + depth * 0.5f, y - height - depth * 0.35f);
		PointF b1 = new(x, y);
		PointF b2 = new(x + width, y);
		PointF b3 = new(x + width + depth * 0.5f, y - depth * 0.35f);

		FillQuad(g, Point(t1), Point(t2), Point(t3), Point(t4), top);
		FillQuad(g, Point(t1), Point(b1), Point(b2), Point(t2), left);
		FillQuad(g, Point(t2), Point(b2), Point(b3), Point(t3), right);
	}

	private static void DrawVignette(Graphics g, int w, int h)
	{
		using System.Drawing.Drawing2D.GraphicsPath path = new();
		path.AddEllipse(-w / 6, -h / 8, w * 4 / 3, h * 5 / 4);
		using System.Drawing.Drawing2D.PathGradientBrush brush = new(path)
		{
			CenterColor = Color.FromArgb(0, 0, 0, 0),
			SurroundColors = new[] { Color.FromArgb(140, 0, 0, 0) }
		};
		g.FillRectangle(brush, 0, 0, w, h);
	}

	private static void FillQuad(Graphics g, Point a, Point b, Point c, Point d, Color color)
	{
		using SolidBrush brush = new(color);
		g.FillPolygon(brush, new[] { a, b, c, d });
		using Pen edge = new(Color.FromArgb(50, 0, 0, 0), 1f);
		g.DrawPolygon(edge, new[] { a, b, c, d });
	}

	private static Point Point(PointF p) => new((int)p.X, (int)p.Y);

	private static Color Darken(Color color, float factor)
	{
		return Color.FromArgb(
			(int)(color.R * factor),
			(int)(color.G * factor),
			(int)(color.B * factor));
	}

	private static class Theme
	{
		public static readonly Color Ceiling = Color.FromArgb(38, 32, 28);
		public static readonly Color Floor = Color.FromArgb(46, 38, 32);
		public static readonly Color LeftWall = Color.FromArgb(58, 48, 40);
		public static readonly Color RightWall = Color.FromArgb(42, 34, 28);
		public static readonly Color BackWall = Color.FromArgb(32, 26, 22);
	}
}
