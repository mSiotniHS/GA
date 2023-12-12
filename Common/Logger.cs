using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common;

public static class Logger
{
	private static readonly StreamWriter Fs = new("log.txt", true, Encoding.UTF8);

	private static readonly List<(string, string)> Trace = new();

	public static void Log(string message)
	{
#if false
		var (className, methodName) = Trace.Last();
		Fs.Write($"{GetIndent()}[{className}/{methodName}] {message}{Environment.NewLine}");
#endif
	}

	public static void Begin(string className, string methodName)
	{
		Trace.Add((className, methodName));
		Log("BEGIN");
	}

	public static void End()
	{
		Log("END");
		Trace.RemoveAt(Trace.Count - 1);
	}

	private static string GetIndent()
	{
		var deepness = Trace.Count;

		if (deepness == 0)
			return string.Empty;

		var indent = new StringBuilder();

		indent.Append("{");
		for (var i = 0; i < deepness; i++)
		{
			indent.Append(i + 1);
		}
		indent.Append("} ");

		return indent.ToString();
	}
}
