using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using log4net;
using MiNET.Worlds;

namespace MiNET.Utils
{
	public class Config
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Config));

		public static string ConfigFileName = "server.conf";
		private static IReadOnlyDictionary<string, string> KeyValues { get; set; }

		static Config()
		{
			try
			{
				string username = Environment.UserName;

				string fileContents = string.Empty;
				if (!MiNetServer.IsRunningOnMono()) //Fix issue on linux/mono.
				{
					var assembly = Assembly.GetExecutingAssembly().GetName().CodeBase;
					string rawPath = Path.GetDirectoryName(assembly);
					if (rawPath != null)
					{
						var path = new Uri(rawPath).LocalPath;

						var configFilePath = Path.Combine(path, $"server.{username}.conf");
						Log.Info($"Trying to load config-file {configFilePath}");
						if (File.Exists(configFilePath))
						{
							fileContents = File.ReadAllText(configFilePath);
						}
						else
						{
							configFilePath = Path.Combine(path, ConfigFileName);

							Log.Info($"Trying to load config-file {configFilePath}");

							if (File.Exists(configFilePath))
							{
								fileContents = File.ReadAllText(configFilePath);
							}
						}
						Log.Info($"Loading config-file {configFilePath}");
					}
				}
				else
				{
					if (File.Exists(ConfigFileName))
					{
						fileContents = File.ReadAllText(ConfigFileName);
					}
				}

				LoadValues(fileContents);
			}
			catch (Exception e)
			{
				Log.Warn("Error configuring parser", e);
			}
		}

		private static void LoadValues(string data)
		{
			Dictionary<string, string> newDictionairy = new Dictionary<string, string>();
			foreach (
				string rawLine in data.Split(new[] {"\r\n", "\n", Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
			{
				string line = rawLine.Trim();
				if (line.StartsWith("#") || !line.Contains("=")) continue; //It's a comment or not a key value pair.

				string[] splitLine = line.Split('=');

				string key = splitLine[0].ToLower();
				string value = splitLine[1];
				Log.Debug($"{key}={value}");
				if (!newDictionairy.ContainsKey(key))
				{
					newDictionairy.Add(key, value);
				}
			}
			KeyValues = new ReadOnlyDictionary<string, string>(newDictionairy);
		}

		public static ServerRole GetProperty(string property, ServerRole defaultValue)
		{
			string value = ReadString(property);
			if (value == null) return defaultValue;

			switch (value.ToLower())
			{
				case "1":
				case "node":
					return ServerRole.Node;
				case "0":
				case "proxy":
					return ServerRole.Proxy;
				case "2":
				case "full":
					return ServerRole.Full;
				default:
					return defaultValue;
			}
		}

		public static GameMode GetProperty(string property, GameMode defaultValue)
		{
			string value = ReadString(property);
			if (value == null) return defaultValue;

			switch (value.ToLower())
			{
				case "0":
				case "survival":
					return GameMode.Survival;
				case "1":
				case "creative":
					return GameMode.Creative;
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

		public static Boolean GetProperty(string property, bool defaultValue)
		{
			try
			{
				string d = ReadString(property);
				if (d == null) return defaultValue;

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
				var value = ReadString(property);
				if (value == null) return defaultValue;

				return Convert.ToInt32(value);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static Difficulty GetProperty(string property, Difficulty defaultValue)
		{
			string df = ReadString(property);

			if (df == null) return defaultValue;

			switch (df.ToLower())
			{
				case "0":
				case "peaceful":
					return Difficulty.Peaceful;
				case "1":
				case "easy":
					return Difficulty.Easy;
				case "2":
				case "normal":
					return Difficulty.Normal;
				case "3":
				case "hard":
					return Difficulty.Hard;
				case "hardcore":
					return Difficulty.Hardcore;
				default:
					return defaultValue;
			}
		}

		public static string GetProperty(string property, string defaultValue)
		{
			return ReadString(property) ?? defaultValue;
		}

		private static string ReadString(string property)
		{
			property = property.ToLower();
			if (!KeyValues.ContainsKey(property)) return null;
			return KeyValues[property];
		}
	}
}