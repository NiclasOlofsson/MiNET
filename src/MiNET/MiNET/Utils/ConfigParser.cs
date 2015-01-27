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

        public static GameMode GetProperty(string Property, GameMode DefaultValue)
        {
            return ReadGamemode(Property, DefaultValue);
        }

        public static Boolean GetProperty(string Property, Boolean DefaultValue)
        {
            return ReadBoolean(Property, DefaultValue);
        }

        public static int GetProperty(string Property, int DefaultValue)
        {
            return ReadInt(Property, DefaultValue);
        }

        public static Difficulty GetProperty(string Property, Difficulty DefaultValue)
        {
            return ReadDifficulty(Property, DefaultValue);
        }

        public static string GetProperty(string Property, string DefaultValue)
        {
            try
            {
                return ReadString(Property);
            }
            catch
            {
                return DefaultValue;
            }
        }

        private static string ReadString(string Rule)
		{
			foreach (string Line in FileContents.Split(new string[] {"\r\n", "\n", Environment.NewLine}, StringSplitOptions.None))
			{
				if (Line.ToLower().StartsWith(Rule.ToLower() + "="))
				{
					string Value = Line.Split('=')[1];
					return Value.ToLower();
				}
			}
            throw new EntryPointNotFoundException("The specified property was not found.");
		}

        private static int ReadInt(string Rule, int Default)
		{
            try
            {
                return Convert.ToInt32(ReadString(Rule));
            }
            catch
            {
                return Default;
            }
		}

		private static bool ReadBoolean(string Rule, Boolean Default)
		{
            try
            {
			    string D = ReadString(Rule);
                return Convert.ToBoolean(D);
            }
            catch
            {
                return Default;
            }
		}

		private static GameMode ReadGamemode(string Rule, GameMode Default)
		{
            try
            {
                string gm = ReadString(Rule);
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
    					return Default;
    			}
            }
            catch
            {
                return Default;
            }
		}

		private static Difficulty ReadDifficulty(string Rule, Difficulty Default)
		{
            try
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
                        return Default;
    			}
            }
            catch
            {
                return Default;
            }
		}
	}
}