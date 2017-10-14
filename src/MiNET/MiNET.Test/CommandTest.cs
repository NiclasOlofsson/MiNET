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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.Utils.Skins;
using MiNET.Worlds;
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
		public void GeometryParserTest()
		{
			var geometryModel = Skin.ParseGeometry(Path.Combine(TestContext.CurrentContext.TestDirectory, "geometry.json"));

			Assert.NotNull(geometryModel);

			Assert.NotNull(geometryModel.First().Value.Bones);
			Assert.AreEqual(BoneName.Head, geometryModel.First().Value.Bones[2].Name);

			Console.WriteLine(Skin.ToJson(geometryModel));
		}


		[Test, Ignore("")]
		public void ParseTest()
		{
			string commandJson = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\test_commands_1.json");

			dynamic dynaCommands = JObject.Parse(commandJson);

			var t = dynaCommands.difficulty.versions[0].overloads["byName"].output.format_strings;

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

		[Test, Ignore("")]
		public void ParseObjectModelTest()
		{
			string commandJson = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\test_commands_1.json");

			JsonConvert.DefaultSettings = () =>
			{
				var settings = new JsonSerializerSettings();
				return settings;
			};
			var commands = JsonConvert.DeserializeObject<CommandSet>(commandJson /*, new ParameterConverter()*/);

			Assert.AreEqual(72, commands.Count);

			Assert.AreEqual("difficulty", commands.First(pair => pair.Key == "difficulty").Key);

			Command command = commands["difficulty"];
			Assert.NotNull(command);

			Version version = command.Versions[0];
			Assert.AreEqual("commands.difficulty.usage", version.Description);

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
			Assert.AreEqual("difficulty", inputParameter.Name);
			Assert.AreEqual("stringenum", inputParameter.Type);
			//Assert.AreEqual("commandName", inputParameter.EnumType);
			Assert.IsFalse(inputParameter.Optional);

			// Output
			Output output = overload.Output;

			Assert.NotNull(output.FormatStrings);
			Assert.AreEqual(1, output.FormatStrings.Length);
			Assert.AreEqual("commands.difficulty.success", output.FormatStrings[0].Format);
			Assert.NotNull(output.Parameters);
			Assert.AreEqual(1, output.Parameters.Length);

			Parameter outputParameter = output.Parameters[0];

			Assert.AreEqual("difficulty", outputParameter.Name);
			Assert.AreEqual("string", outputParameter.Type);
			Assert.That(outputParameter.EnumType, Is.Null.Or.Empty);
			Assert.IsFalse(outputParameter.Optional);
		}

		[Test, Ignore("")]
		public void SerializeObjectModelRoundtripTest()
		{
			string commandJson = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\test_commands_1.json");

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var commands = JsonConvert.DeserializeObject<CommandSet>(commandJson, settings);
			Assert.AreEqual("difficulty", commands.First(pair => pair.Key == "difficulty").Key);

			Command command = commands["difficulty"];
			Assert.NotNull(command);

			CommandSet toSerialize = new CommandSet();
			toSerialize["difficulty"] = command;

			var output = JsonConvert.SerializeObject(toSerialize, settings);
			Console.WriteLine($"{output}");
		}

		[Test, Ignore("")]
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

		[Test]
		public void ParseCommandRequst12()
		{
			string cmd = @"/test @e[t=2] a ""b"" c'c2 ""d"" f";

			var split = Regex.Split(cmd, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").Select(s => s.Trim('"')).ToArray();
			Assert.AreEqual(new[] {"/test", "@e[t=2]", "a", "b", "c'c2", "d", "f"}, split);
		}

		[Test, Ignore("")]
		public void ParseCommandSelectors12()
		{
			string cmd = @"@e[test=123,ugh=456]";

			var split = Regex.Split(cmd, @"^(@[aeprs])\[(\w*?=\w*?,??)*\]$").Where(s => s != string.Empty).ToArray();
			Assert.AreEqual(new[] {"@e", "test=123", "ugh=456"}, split);
		}

		[Test]
		public void ParseCommandSelectorsNoArguments12_2()
		{
			string source = @"@e";

			var matches = Regex.Matches(source, @"^(?<selector>@[aeprs])(\[((?<args>(c|dx|dy|dz|l|lm|m|name|r|rm|rx|rxm|rym|type|x|y|z)=.*?)(,*?))*\])*$");
			Assert.AreEqual("@e", matches[0].Groups["selector"].Captures[0].Value);
		}

		[Test]
		public void ParseCommandSelectors12_2()
		{
			string cmd = @"@e[test=123,ugh=!456,,bah=~,,,,bah=,,,test=1234,test=""sdff""]";

			var matches = Regex.Matches(cmd, @"^(?<selector>@[aeprs])\[((?<args>(test|ugh|bah)=.*?)(,*?))*\]$");
			Assert.AreEqual("@e", matches[0].Groups["selector"].Captures[0].Value);
			Assert.AreEqual("test=123", matches[0].Groups["args"].Captures[0].Value);
			Assert.AreEqual("ugh=!456", matches[0].Groups["args"].Captures[1].Value);
			Assert.AreEqual("bah=~", matches[0].Groups["args"].Captures[2].Value);
			Assert.AreEqual("bah=", matches[0].Groups["args"].Captures[3].Value);
			Assert.AreEqual("test=1234", matches[0].Groups["args"].Captures[4].Value);
			Assert.AreEqual(@"test=""sdff""", matches[0].Groups["args"].Captures[5].Value);
		}

		[Test]
		public void TargetWithPlayerParserTest()
		{
			Target target = PluginManager.ParseTarget("gurunx");
			Assert.AreEqual("closestPlayer", target.Selector);
			Assert.AreEqual("gurunx", target.Rules.First().Value);
		}

		[Test]
		public void TargetWithSelectorsParserTest()
		{
			Target target = null;

			target = PluginManager.ParseTarget("@a");
			Assert.AreEqual("allPlayers", target.Selector);
			Assert.IsNull(target.Rules);

			target = PluginManager.ParseTarget("@a[name=gurunx,name=oliver]");
			Assert.AreEqual("allPlayers", target.Selector);
			Assert.AreEqual(false, target.Rules.First().Inverted);
			Assert.AreEqual("gurunx", target.Rules.First().Value);
			Assert.AreEqual(false, target.Rules.Skip(1).First().Inverted);
			Assert.AreEqual("oliver", target.Rules.Skip(1).First().Value);

			target = PluginManager.ParseTarget("@a[name=!gurunx,name=!oliver]");
			Assert.AreEqual("allPlayers", target.Selector);
			Assert.AreEqual(true, target.Rules.First().Inverted);
			Assert.AreEqual("gurunx", target.Rules.First().Value);
			Assert.AreEqual(true, target.Rules.Skip(1).First().Inverted);
			Assert.AreEqual("oliver", target.Rules.Skip(1).First().Value);

		}

	}
}