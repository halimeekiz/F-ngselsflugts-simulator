using Fængselsflugts_simulator.Enums;

namespace Fængselsflugts_simulator.Gui;

internal sealed class PrisonMapPanel : Panel
{
	private PrisonLocation current = PrisonLocation.CellBlockA;
	private bool alarmOn;
	private float pulse;

	public PrisonMapPanel()
	{
		DoubleBuffered = true;
		BackColor = Color.FromArgb(14, 12, 11);
		ResizeRedraw = true;
	}

	public void SetState(PrisonLocation location, bool alarmOn, float pulse)
	{
		current = location;
		this.alarmOn = alarmOn;
		this.pulse = pulse;
		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics g = e.Graphics;
		g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		g.Clear(BackColor);

		float tileW = Math.Max(18f, Width / 14f);
		float tileH = tileW * 0.52f;
		float originX = Width * 0.50f;
		float originY = Height * 0.10f;

		DrawGround(g, originX, originY, tileW, tileH);

		IsoRoom[] rooms =
		{
			new(PrisonLocation.CellBlockA, 0, 0, 2.2f, 2.0f, 28, "CELLE A"),
			new(PrisonLocation.Cafeteria, 2.4f, 0, 2.2f, 2.0f, 26, "KANTINE"),
			new(PrisonLocation.CellBlockB, 4.8f, 0, 2.2f, 2.0f, 28, "CELLE B"),
			new(PrisonLocation.Corridor, 0, 2.2f, 7.0f, 1.1f, 14, "GANG"),
			new(PrisonLocation.Shower, 0, 3.5f, 3.3f, 1.8f, 22, "BAD"),
			new(PrisonLocation.Yard, 3.5f, 3.5f, 3.5f, 1.8f, 8, "GÅRD"),
			new(PrisonLocation.Medical, 0, 5.5f, 1.7f, 1.8f, 22, "SYGE"),
			new(PrisonLocation.GuardRoom, 1.8f, 5.5f, 1.7f, 1.8f, 24, "VAGT"),
			new(PrisonLocation.ControlRoom, 3.6f, 5.5f, 1.7f, 1.8f, 24, "KONTROL"),
			new(PrisonLocation.Storage, 5.4f, 5.5f, 1.6f, 1.8f, 26, "LAGER"),
			new(PrisonLocation.MainEntrance, 0, 7.5f, 7.0f, 1.2f, 18, "UDGANG")
		};

		foreach (IsoRoom room in rooms.OrderBy(r => r.X + r.Y))
		{
			DrawIsoRoom(g, room, originX, originY, tileW, tileH);
		}
	}

	private void DrawGround(Graphics g, float ox, float oy, float tw, float th)
	{
		Point[] pad =
		{
			Iso(ox, oy, tw, th, -0.4f, -0.4f),
			Iso(ox, oy, tw, th, 7.6f, -0.4f),
			Iso(ox, oy, tw, th, 7.6f, 9.0f),
			Iso(ox, oy, tw, th, -0.4f, 9.0f)
		};
		using SolidBrush dirt = new(Color.FromArgb(26, 22, 18));
		g.FillPolygon(dirt, pad);
	}

	private void DrawIsoRoom(Graphics g, IsoRoom room, float ox, float oy, float tw, float th)
	{
		bool here = current == room.Location;
		float extra = here ? 8 + pulse * 6 : 0;
		float height = room.Height + extra;

		int lift = (int)height;
		Point top1 = Offset(Iso(ox, oy, tw, th, room.X, room.Y), 0, -lift);
		Point top2 = Offset(Iso(ox, oy, tw, th, room.X + room.W, room.Y), 0, -lift);
		Point top3 = Offset(Iso(ox, oy, tw, th, room.X + room.W, room.Y + room.H), 0, -lift);
		Point top4 = Offset(Iso(ox, oy, tw, th, room.X, room.Y + room.H), 0, -lift);
		Point bot1 = Iso(ox, oy, tw, th, room.X, room.Y);
		Point bot2 = Iso(ox, oy, tw, th, room.X + room.W, room.Y);
		Point bot3 = Iso(ox, oy, tw, th, room.X + room.W, room.Y + room.H);
		Point bot4 = Iso(ox, oy, tw, th, room.X, room.Y + room.H);

		Color top = here ? Color.FromArgb(168, 112, 42) : Color.FromArgb(64, 54, 46);
		Color left = here ? Color.FromArgb(118, 72, 28) : Color.FromArgb(46, 38, 32);
		Color right = here ? Color.FromArgb(86, 52, 20) : Color.FromArgb(34, 28, 24);

		if (alarmOn && (room.Location == PrisonLocation.ControlRoom || room.Location == PrisonLocation.MainEntrance))
		{
			int add = 40 + (int)(50 * pulse);
			top = Color.FromArgb(Math.Min(255, top.R + add), top.G / 2, top.B / 2);
		}

		using (SolidBrush shadow = new(Color.FromArgb(70, 0, 0, 0)))
		{
			g.FillPolygon(shadow, new[]
			{
				Offset(bot1, 10, 8),
				Offset(bot2, 10, 8),
				Offset(bot3, 10, 8),
				Offset(bot4, 10, 8)
			});
		}

		Fill(g, new[] { top4, bot4, bot3, top3 }, left);
		Fill(g, new[] { top2, bot2, bot3, top3 }, right);
		Fill(g, new[] { top1, top2, top3, top4 }, top);

		using Font font = new("Segoe UI", here ? 8.5f : 7.2f, FontStyle.Bold);
		using SolidBrush text = new(here ? Color.FromArgb(255, 236, 196) : Color.FromArgb(196, 178, 154));
		Point center = new((top1.X + top3.X) / 2, (top1.Y + top3.Y) / 2 - 6);
		SizeF size = g.MeasureString(room.Label, font);
		g.DrawString(room.Label, font, text, center.X - size.Width / 2, center.Y - size.Height / 2);

		if (here)
		{
			using SolidBrush you = new(Color.FromArgb(255, 214, 90));
			g.FillEllipse(you, center.X - 5, center.Y - 18, 10, 10);
			using SolidBrush body = new(Color.FromArgb(40, 32, 24));
			g.FillEllipse(body, center.X - 4, center.Y - 10, 8, 11);
		}
	}

	private static void Fill(Graphics g, Point[] points, Color color)
	{
		using SolidBrush brush = new(color);
		g.FillPolygon(brush, points);
		using Pen edge = new(Color.FromArgb(80, 0, 0, 0), 1f);
		g.DrawPolygon(edge, points);
	}

	private static Point Iso(float ox, float oy, float tw, float th, float x, float y)
	{
		return new Point(
			(int)(ox + (x - y) * tw),
			(int)(oy + (x + y) * th));
	}

	private static Point Offset(Point p, int x, int y) => new(p.X + x, p.Y + y);

	private readonly record struct IsoRoom(
		PrisonLocation Location,
		float X,
		float Y,
		float W,
		float H,
		float Height,
		string Label);
}
