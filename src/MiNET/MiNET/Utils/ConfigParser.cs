using System;
using System.IO;
using System.Reflection;
using log4net;
using MiNET.Worlds;

namespace MiNET.Utils
{
	public class ConfigParser
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ConfigParser));

		public static string ConfigFileName = "server.conf";
		private static string FileContents = string.Empty;

		static ConfigParser()
		{
			try
			{
				var assembly = Assembly.GetExecutingAssembly().GetName().CodeBase;
				var path = new Uri(Path.GetDirectoryName(assembly)).LocalPath;

				var configFilePath = Path.Combine(path, ConfigFileName);

				if (File.Exists(configFilePath))
				{
					FileContents = File.ReadAllText(configFilePath);
				}
			}
			catch (Exception e)
			{
				Log.Warn("Error configuring parser", e);
			}
		}

		public static GameMode GetProperty(string property, GameMode defaultValue)
		{
			return ReadGamemode(property, defaultValue);
		}

		public static Boolean GetProperty(string property, Boolean defaultValue)
		{
			return ReadBoolean(property, defaultValue);
		}

		public static int GetProperty(string property, int defaultValue)
		{
			return ReadInt(property, defaultValue);
		}

		public static Difficulty GetProperty(string property, Difficulty defaultValue)
		{
			return ReadDifficulty(property, defaultValue);
		}

		public static string GetProperty(string property, string defaultValue)
		{
			try
			{
				return ReadString(property);
			}
			catch
			{
				return defaultValue;
			}
		}

		private static string ReadString(string rule)
		{
			foreach (string line in FileContents.Split(new[] {"\r\n", "\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
			{
				if (line.ToLower().StartsWith(rule.ToLower() + "="))
				{
					string value = line.Split('=')[1];
					return value.ToLower();
				}
			}
			throw new EntryPointNotFoundException("The specified property was not found.");
		}

		private static int ReadInt(string rule, int defaultValue)
		{
			try
			{
				return Convert.ToInt32(ReadString(rule));
			}
			catch
			{
				return defaultValue;
			}
		}

		private static bool ReadBoolean(string rule, Boolean defaultValue)
		{
			try
			{
				string d = ReadString(rule);
				return Convert.ToBoolean(d);
			}
			catch
			{
				return defaultValue;
			}
		}

		private static GameMode ReadGamemode(string rule, GameMode defaultValue)
		{
			try
			{
				string gm = ReadString(rule);
				switch (gm)
				{
					case "1":
					case "creative":
						return GameMode.Creative;
					case "0":
					case "survival":
						return GameMode.Survival;
					case "2":
					case "adventure":
						return GameMode.Adventure;
					case "3":
					case "spectator":
						return GameMode.Spectator;
					default:
						return defaultValue;
				}
			}
			catch
			{
				return defaultValue;
			}
		}

		private static Difficulty ReadDifficulty(string rule, Difficulty defaultValue)
		{
			try
			{
				string df = ReadString(rule).ToLower();
				switch (df)
				{
					case "easy":
						return Difficulty.Easy;
					case "normal":
						return Difficulty.Normal;
					case "hard":
						return Difficulty.Hard;
					case "peaceful":
						return Difficulty.Peaceful;
					default:
						return defaultValue;
				}
			}
			catch
			{
				return defaultValue;
			}
		}
	}
}