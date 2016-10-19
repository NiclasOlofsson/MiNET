using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiNET.Blocks;
using MiNET.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Version = MiNET.Plugins.Version;

namespace MiNET
{
	[TestFixture]
	public class CommandTest
	{
		[Test]
		public void ParseTest()
		{
			string commandJson = File.ReadAllText("test_commands_1.json");

			dynamic dynaCommands = JObject.Parse(commandJson);

			var t = dynaCommands.ability.versions[0].overloads["default"].output.format_strings;

			foreach (var te in dynaCommands)
			{
				foreach (var tis in te)
				{
					//Console.WriteLine($"{tis}");
					foreach (var version in tis.versions)
					{
						Console.WriteLine($"{version.description}");	
					}

				}
			}
			
			//JsonConvert.DefaultSettings = () =>
			//{
			//	var settings = new JsonSerializerSettings();
			//	return settings;
			//};
			//var commands = JsonConvert.DeserializeObject<Dictionary<string, Command>>(commandJson/*, new ParameterConverter()*/);

			//Assert.AreEqual(36, commands.Count);

			//Assert.AreEqual("ability", commands.First().Key);

			//Command command = commands.First().Value;
			//Assert.NotNull(command);

			//Version version = command.Versions[0];
			//Assert.AreEqual("commands.ability.description", version.Description);

			//Assert.AreEqual(1, version.Overloads.Count);
			//Assert.NotNull(version.Overloads["default"]);

			//Console.WriteLine($"{version.Overloads["default"].Input[0]}");
			//Console.WriteLine($"{version.Overloads["default"].Output[0]}");
			//Console.WriteLine($"{version.Overloads["default"].Output[0]}");

			//var input = version.Overloads["default"].Configurations["input"];

			//Assert.NotNull(input);
		}

	}


}