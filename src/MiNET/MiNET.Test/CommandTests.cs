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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Globalization;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Effects;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace MiNET.Test
{
	[TestClass]
	public class CommandTests
	{
		[TestInitialize]
		public void Initialize()
		{
			// We do this to get the number formatting to work proper under these tests
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		}

		[TestMethod]
		public void IntCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdInt));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"1", "2", "3"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("1, 2, 3", result);
		}

		[TestMethod]
		public void FloatCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdFloat));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"1.2", "3.4"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("1.2, 3.4", result);
		}

		[TestMethod]
		public void BoolCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdBool));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"true", "false"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("True, False", result);
		}

		[TestMethod]
		public void StringCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdString));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"text"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("text", result);
		}

		[TestMethod]
		public void BlockPosCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdBlockPos));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"1", "2", "~3"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("X: 1, XRelative: False, Y: 2, YRelative: False, Z: 3, ZRelative: True", result);
		}

		[TestMethod]
		public void EntityPosCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdEntityPos));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"1.2", "3.4", "~5.6"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("X: 1.2, XRelative: False, Y: 3.4, YRelative: False, Z: 5.6, ZRelative: True", result);
		}

		[TestMethod]
		public void RelValueCommandTest()
		{
			// arrange
			TestPlugin plugin = GetTestPlugin();
			MethodInfo method = plugin.GetMethodInfo(nameof(TestPlugin.CmdRelValue));

			// act
			bool successful = plugin.Manager.ExecuteCommand(method, new Player(null, null), new[] {"1.2", "~3.4"}, out object result);

			// assert
			Assert.IsTrue(successful);
			Assert.AreEqual("Value: 1.2, Relative: False, Value: 3.4, Relative: True", result);
		}

		private static TestPlugin GetTestPlugin()
		{
			var manager = new PluginManager();
			var plugin = new TestPlugin();
			plugin.Manager = manager;
			manager.Plugins.Add(plugin);

			return plugin;
		}
	}


	public class TestPlugin
	{
		public PluginManager Manager { get; set; }

		public MethodInfo GetMethodInfo(string name)
		{
			return GetType().GetMethod(name);
		}

		[Command]
		public string CmdInt(int i, short s, byte b)
		{
			return $"{i}, {s}, {b}";
		}

		[Command]
		public string CmdFloat(float f, double d)
		{
			return $"{f}, {d}";
		}

		[Command]
		public string CmdBool(bool b, bool c)
		{
			return $"{b}, {c}";
		}

		[Command]
		public string CmdString(string s)
		{
			return $"{s}";
		}

		[Command]
		public string CmdRawText(string[] raw)
		{
			return $"{string.Join(',', raw)}";
		}

		[Command]
		public string CmdRawTextParams(params string[] args)
		{
			return $"{string.Join(',', args)}";
		}

		[Command]
		public string CmdTarget(Target t)
		{
			return $"{t}";
		}

		[Command]
		public string CmdBlockPos(BlockPos pos)
		{
			return $"{pos}";
		}

		[Command]
		public string CmdEntityPos(EntityPos pos)
		{
			return $"{pos}";
		}

		[Command]
		public string CmdRelValue(RelValue value, RelValue value2)
		{
			return $"{value}, {value2}";
		}

		[Command]
		public string CmdEnum1(ItemTypeEnum itemType)
		{
			return $"{itemType}";
		}

		[Command]
		public string CmdEnum2(EntityTypeEnum entityType)
		{
			return $"{entityType}";
		}

		[Command]
		public string CmdEnum3(BlockTypeEnum blockType)
		{
			return $"{blockType}";
		}

		[Command]
		public string CmdEnum4(CommandNameEnum commandName)
		{
			return $"{commandName}";
		}

		[Command]
		public string CmdEnum5(EnchantEnum enchant)
		{
			return $"{enchant}";
		}

		[Command]
		public string CmdEnum6(EffectEnum effect)
		{
			return $"{effect}";
		}

		[Command]
		public string CmdEnum7(DimensionEnum dimensions)
		{
			return $"{dimensions}";
		}

		[Command]
		public string CmdEnum8(FeatureEnum feature)
		{
			return $"{feature}";
		}

		[Command]
		public string CmdEnum9(EffectType effects)
		{
			return $"{effects}";
		}

		[Command]
		public string CmdOptional(int i, int optional = 0)
		{
			return $"{i}, {optional}";
		}

		[Command]
		public string CmdReturnString()
		{
			return "Success";
		}
	}
}