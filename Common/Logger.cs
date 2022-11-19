using System;
using System.IO;
using System.Text;

namespace Common;

public static class Logger
{
	private static readonly StreamWriter Fs = new("log.txt", true, Encoding.UTF8);

	private static string _className = string.Empty;
	private static string _methodName = string.Empty;
	private static uint _deepness = 0;

	public static void Log(string message)
	{
		Fs.Write($"{GetIndent()}[{_className}/{_methodName}] {message}{Environment.NewLine}");
	}

	public static void Begin(string className, string methodName)
	{
		_className = className;
		_methodName = methodName;
		_deepness++;

		Log("BEGIN");
	}

	public static void End()
	{
		Log("END");

		_className = string.Empty;
		_methodName = string.Empty;
		_deepness--;
	}

	private static string GetIndent()
	{
		if (_deepness == 0)
			return string.Empty;

		var indent = new StringBuilder();

		for (var i = 0; i < _deepness; i++)
		{
			indent.Append(i + 1);
		}

		return indent.ToString();
	}
}
