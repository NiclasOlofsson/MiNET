﻿#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

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
		private static readonly ILog Log = LogManager.GetLogger(typeof(Config));

		public static string ConfigFileName = "server.conf";
		private static IReadOnlyDictionary<string, string> KeyValues { get; set; }

		static Config()
		{
			try
			{
				string username = Environment.UserName;

				string fileContents = string.Empty;
				string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				if (path != null)
				{
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

				string[] splitLine = line.Split('=', 2);

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