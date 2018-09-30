#region LICENSE

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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using Microsoft.Extensions.Configuration;
using MiNET.Worlds;

namespace MiNET.Console.Config
{
	internal class ConfigParser
	{
		private readonly IConfiguration _config;

		public ConfigParser(IConfiguration config)
		{
			_config = config;
		}

		public ServerRole GetProperty(string property, ServerRole defaultValue)
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

		public GameMode GetProperty(string property, GameMode defaultValue)
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

		public bool GetProperty(string property, bool defaultValue)
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

		public int GetProperty(string property, int defaultValue)
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

		public Difficulty GetProperty(string property, Difficulty defaultValue)
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

		public string GetProperty(string property, string defaultValue)
		{
			return ReadString(property) ?? defaultValue;
		}

		private string ReadString(string property)
		{
			return _config[property];
		}
	}
}