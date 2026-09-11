using System.Text;

namespace Fængselsflugts_simulator.Gui;

internal sealed class LogTextWriter : TextWriter
{
	private readonly Action<string> write;
	private readonly StringBuilder buffer = new();

	public LogTextWriter(Action<string> write)
	{
		this.write = write;
	}

	public override Encoding Encoding => Encoding.UTF8;

	public override void Write(char value)
	{
		if (value == '\n')
		{
			FlushLine();
			return;
		}

		if (value != '\r')
		{
			buffer.Append(value);
		}
	}

	public override void Write(string? value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return;
		}

		foreach (char c in value)
		{
			Write(c);
		}
	}

	public override void WriteLine(string? value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			buffer.Append(value);
		}

		FlushLine();
	}

	private void FlushLine()
	{
		string line = buffer.ToString();
		buffer.Clear();
		write(line);
	}
}
