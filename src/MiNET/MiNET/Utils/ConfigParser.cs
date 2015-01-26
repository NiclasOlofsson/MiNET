using System;
using System.IO;
using MiNET.Worlds;

namespace MiNET.Utils
{
	public class ConfigParser
	{
		public static string ConfigFile = string.Empty;
		private static string FileContents = string.Empty;
		public static string[] InitialValue;

		public ConfigParser()
		{
		}

		public static bool Check()
		{
			if (!File.Exists(ConfigFile))
			{
				File.WriteAllLines(ConfigFile, InitialValue);
				return Check();
			}
			else
			{
				FileContents = File.ReadAllText(ConfigFile);
				if (!FileContents.Contains("#DO NOT REMOVE THIS LINE - MiNET Config"))
				{
					File.Delete(ConfigFile);
					return Check();
				}
				else
				{
					return true;
				}
			}
		}

		public static string ReadString(string Rule)
		{
			foreach (string Line in FileContents.Split(new string[] {"\r\n", "\n", Environment.NewLine}, StringSplitOptions.None))
			{
				if (Line.ToLower().StartsWith(Rule.ToLower() + "="))
				{
					string Value = Line.Split('=')[1];
					return Value.ToLower();
				}
			}
			return "";
		}

		public static int ReadInt(string Rule)
		{
			foreach (string Line in FileContents.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None))
			{
				if (Line.ToLower().StartsWith(Rule.ToLower() + "="))
				{
					string Value = Line.Split('=')[1];
					return Convert.ToInt32(Value);
				}
			}
			return 0;
		}

		public static bool ReadBoolean(string Rule)
		{
			string D = ReadString(Rule);
			if (D == "true")
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static GameMode ReadGamemode(string Rule)
		{
			string gm = ReadString(Rule).ToLower();
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
					return GameMode.Survival;
			}
		}

		public static Difficulty ReadDifficulty(string Rule)
		{
			string df = ReadString(Rule).ToLower();
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
					return Difficulty.Normal;
			}
		}
	}
}