using System;

public class Logging
{
	public enum Level
	{
		Verbose, Info, Warning, Error
	}

	public static Level LogLevel = (Level)Enum.Parse(typeof(Level), (Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "Verbose"));

	public static bool ContainsLevel(Level level)
	{
		return level >= LogLevel;
	}

	public static void Verbose(string tag, string text, params object[] args)
	{
		writeLog(Level.Verbose, tag, text, args);
	}

	public static void Info(string tag, string text, params object[] args)
	{
		writeLog(Level.Info, tag, text, args);
	}

	public static void Warn(string tag, string text, params object[] args)
	{
		writeLog(Level.Warning, tag, text, args);
	}

	public static void Error(string tag, string text, params object[] args)
	{
		writeLog(Level.Error, tag, text, args);
	}

	static void writeLog(Level level, string tag, string text, params object[] args)
	{
		if (level >= LogLevel)
		{
			if (args != null && args.Length > 0)
				text = string.Format(text, args);
			Console.WriteLine("[{0}, {1}, {2}] {3}", DateTime.UtcNow.ToString("yyyy.MM.dd-HH:mm:ss.fff"), level, tag, text);
		}
	}
}
