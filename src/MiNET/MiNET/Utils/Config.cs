using System;
using System.IO;
using System.Net;
using System.Reflection;
using log4net;
using MiNET.Worlds;

namespace MiNET.Utils
{
	public class Config
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Config));

		public static string ConfigFileName = "server.conf";
		private static string FileContents = string.Empty;
	    private static string FileDir = string.Empty;

		static Config()
		{
			try
			{
				if (!MiNetServer.IsRunningOnMono()) //Fix issue on linux/mono.
				{
					var assembly = Assembly.GetExecutingAssembly().GetName().CodeBase;
					var path = new Uri(Path.GetDirectoryName(assembly)).LocalPath;
                    var configFilePath = Path.Combine(path, ConfigFileName);
				    FileDir = ConfigFileName;
                    Log.Info("TEETET");
				    if (File.Exists(configFilePath))
				    {
				        FileContents = File.ReadAllText(configFilePath);
				    }
				    else
				    {
				        Log.WarnFormat("No Config File");
				        Log.WarnFormat("Creating Config File");
                        var pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
				        File.Create(configFilePath).Close();
				        FileContents = ("ip=0.0.0.0\r\n" +
				                        "port=19132\r\n" +
				                        "EnableSecurity=false\r\n" +
				                        "WorldSave=false\r\n" +
				                        "motd=MiNET: MCPE Server\r\n" +
				                        "PluginDisabled=false\r\n" +
				                        "PluginDirectory=pluginDirectory\r\n" +
				                        "PCWorldFolder=World\r\n" +
				                        "PCWaterOffset=0\r\n" +
				                        "seed=noise\r\n" +
				                        "GameMode=0\r\n" +
				                        "Difficulty=0\r\n" +
				                        "ViewDistance=250\r\n" +
				                        "WorldProvider=flat\r\n");
                        Config.SaveAll();
				    }
				}
				else
				{
					if (File.Exists(ConfigFileName))
					{
						FileContents = File.ReadAllText(ConfigFileName);
					}
				}
			}
			catch (Exception e)
			{
				Log.Warn("Error configuring parser", e);
			}
		}

		public static GameMode GetProperty(string property, GameMode defaultValue)
		{
			try
			{
				string gm = ReadString(property);
				switch (gm.ToLower())
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

	    public static Boolean SaveAll()
	    {
            File.WriteAllText(FileDir,FileContents.ToString());
            return true;
	    }

		public static Boolean GetProperty(string property, bool defaultValue)
		{
			try
			{
				string d = ReadString(property);
				return Convert.ToBoolean(d);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static int GetProperty(string property, int defaultValue)
		{
			try
			{
				return Convert.ToInt32(ReadString(property));
			}
			catch
			{
				return defaultValue;
			}
		}

		public static Difficulty GetProperty(string property, Difficulty defaultValue)
		{
			try
			{
				string df = ReadString(property).ToLower();
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

        public static Boolean SetBoolean(string key, int val)
        {
            foreach (string line in FileContents.Split(new[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.ToLower().StartsWith(key.ToLower() + "="))
                {
                    string newline = key + "=" + val;
                    FileContents.Replace(line, newline);
                    return true;
                }
            }
            return false;
        }

        public static Boolean SetBoolean(string key, bool val)
        {
            foreach (string line in FileContents.Split(new[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.ToLower().StartsWith(key.ToLower() + "="))
                {
                    string newline = key + "=" + val;
                    FileContents.Replace(line, newline);
                    return true;
                }
            }
            return false;
        }

        public static Boolean SetString(string key, string val)
	    {
            foreach (string line in FileContents.Split(new[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.ToLower().StartsWith(key.ToLower() + "="))
                {
                    string newline = key+"="+val;
                    FileContents.Replace(line, newline);
                    return true;
                }
            }
	        return false;
	    }

		private static string ReadString(string property)
		{
			foreach (string line in FileContents.Split(new[] {"\r\n", "\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
			{
				if (line.ToLower().StartsWith(property.ToLower() + "="))
				{
					string value = line.Split('=')[1];
					return value;
				}
			}
			throw new EntryPointNotFoundException("The specified property was not found.");
		}
	}
}