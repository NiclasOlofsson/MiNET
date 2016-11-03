using System;
using System.IO;
using System.Linq;
using MiNET.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
		}

		[Test]
		public void ParseObjectModelTest()
		{
			string commandJson = File.ReadAllText("test_commands_1.json");

			JsonConvert.DefaultSettings = () =>
			{
				var settings = new JsonSerializerSettings();
				return settings;
			};
			var commands = JsonConvert.DeserializeObject<Commands>(commandJson /*, new ParameterConverter()*/);

			Assert.AreEqual(36, commands.Count);

			Assert.AreEqual("ability", commands.First().Key);

			Command command = commands["help"];
			Assert.NotNull(command);

			Version version = command.Versions[0];
			Assert.AreEqual("commands.help.description", version.Description);

			Assert.AreEqual(2, version.Overloads.Count);

			Overload overload = version.Overloads["byName"];
			Assert.NotNull(overload);

			// Input
			Input input = overload.Input;

			Assert.NotNull(input);
			Assert.NotNull(input.Parameters);
			Assert.AreEqual(1, input.Parameters.Length);

			// Input parameters
			Parameter inputParameter = input.Parameters[0];

			Assert.NotNull(inputParameter);
			Assert.AreEqual("command", inputParameter.Name);
			Assert.AreEqual("stringenum", inputParameter.Type);
			Assert.AreEqual("commandName", inputParameter.EnumType);
			Assert.IsFalse(inputParameter.Optional);

			// Output
			Output output = overload.Output;

			Assert.NotNull(output.FormatStrings);
			Assert.AreEqual(1, output.FormatStrings.Length);
			Assert.AreEqual("{0}", output.FormatStrings[0]);
			Assert.NotNull(output.Parameters);
			Assert.AreEqual(1, output.Parameters.Length);

			Parameter outputParameter = output.Parameters[0];

			Assert.AreEqual("body", outputParameter.Name);
			Assert.AreEqual("string", outputParameter.Type);
			Assert.IsNullOrEmpty(outputParameter.EnumType);
			Assert.IsFalse(outputParameter.Optional);
		}

		[Test]
		public void SerialzeObjectModelTest()
		{
			string commandJson = File.ReadAllText("test_commands_1.json");

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var commands = JsonConvert.DeserializeObject<Commands>(commandJson, settings);
			Assert.AreEqual("ability", commands.First().Key);

			Command command = commands["help"];
			Assert.NotNull(command);

			Commands toSerialize = new Commands();
			toSerialize["help"] = command;

			var output = JsonConvert.SerializeObject(toSerialize, settings);
			Console.WriteLine($"{output}");
		}
	}
}