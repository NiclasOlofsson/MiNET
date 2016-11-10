using System;
using System.IO;
using System.Linq;
using System.Text;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using TestPlugin;
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
			var commands = JsonConvert.DeserializeObject<CommandSet>(commandJson /*, new ParameterConverter()*/);

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
		public void SerializeObjectModelRoundtripTest()
		{
			string commandJson = File.ReadAllText("test_commands_1.json");

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var commands = JsonConvert.DeserializeObject<CommandSet>(commandJson, settings);
			Assert.AreEqual("ability", commands.First().Key);

			Command command = commands["help"];
			Assert.NotNull(command);

			CommandSet toSerialize = new CommandSet();
			toSerialize["help"] = command;

			var output = JsonConvert.SerializeObject(toSerialize, settings);
			Console.WriteLine($"{output}");
		}

		[Test]
		public void DeserializeObjectModelTest()
		{
			CommandSet commandSet = PluginManager.GenerateCommandSet(typeof (CoreCommands).GetMethods());

			string json =
				@"
{
  ""x"": 1,
  ""y"": 2,
  ""z"": 3
}
";

			var commandJson = JsonConvert.DeserializeObject<dynamic>(json);

			PluginManager pm = new PluginManager();
			pm.Commands = commandSet;
			pm.HandleCommand(null, "tp", "default", commandJson);

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var output = JsonConvert.SerializeObject(commandSet, settings);
			Console.WriteLine($"{output}");
		}


		[Test]
		public void SerializeTest()
		{
			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			StringBuilder sb = new StringBuilder();
			sb.Append("test\n");
			sb.Append("tes2");

			var output = JsonConvert.SerializeObject(new HelpCommand.HelpResponseByPage() {Body = sb.ToString()}, settings);
			Console.WriteLine($"{output}");
			string test =
				@"
			{
				""page"": 10,
			}";

			JObject dyn = (JObject) JsonConvert.DeserializeObject(test);

			Assert.True(PluginManager.HasProperty(dyn, "page"));
		}

		[Command(Name = "tp", Description = "Teleports player to given coordinates")]
		public void Teleport(Player player, int x, int y, int z)
		{
		}

		[Test]
		public void CommandUsageTest()
		{
			CommandSet commandSetIn = PluginManager.GenerateCommandSet(typeof (HelpCommand).GetMethods());

			int page = 2;
			var commands = commandSetIn;
			//var commands = commandsIn.Skip(page*7);
			int i = 0;
			foreach (var command in commands)
			{
				string result = PluginManager.GetUsage(command.Value).Replace("\n", "\\n") + "\\n";
				Console.Write(result);
				if (i++ >= 6) break;
			}

			//Console.WriteLine("Commands:");
			//foreach (var command in commands)
			//{
			//	string result = PluginManager.GetUsage(command.Key, command.Value);
			//	Console.WriteLine(result);
			//}
			//{
			//	string result = PluginManager.GetUsage("help", commands["help"]);

			//	//Assert.AreEqual("/help", result.Split('\n')[0]);
			//	Console.WriteLine(result);

			//}
		}
	}
}