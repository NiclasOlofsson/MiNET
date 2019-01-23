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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using System.Text;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace MiNET.Plugins.Commands
{
	public class HelpCommand
	{
		private readonly PluginManager _pluginManager;

		public HelpCommand(PluginManager pluginManager)
		{
			_pluginManager = pluginManager;
		}

		public class HelpResponseByName
		{
			public string Body { get; set; }
			public int StatusCode { get; set; }
		}

		[Command(Description = "commands.help.description", OutputFormatStrings = new[] {"{0}"})]
		public HelpResponseByName Help(Player player, CommandNameEnum command)
		{
			Command cmd;
			if (_pluginManager.Commands.TryGetValue(command.Value, out cmd))
			{
				string[] aliases = cmd.Versions.First().Aliases;
				string cmds = "";
				if (aliases != null)
				{
					cmds = ", " + string.Join(", ", aliases);
				}
				StringBuilder sb = new StringBuilder();
				sb.Append($"{ChatColors.Green}{cmd.Name}{cmds}:{ChatFormatting.Reset}\n");
				sb.Append($"Usage:\n{PluginManager.GetUsage(cmd, true, ChatColors.Aqua)}");

				return new HelpResponseByName() {Body = sb.ToString()};
			}

			return null;
		}

		public class HelpResponseByPage
		{
			public int Page { get; set; }
			public int PageCount { get; set; }
			public string Body { get; set; }
			public int StatusCode { get; set; }
			public int SuccessCount { get; set; } = 1;
		}

		[Command(Description = "commands.help.description", OutputFormatStrings = new[] {"commands.help.header", "{2}", "commands.help.footer"})]
		public HelpResponseByPage Help(Player player, int page = 0)
		{
			page = page < 1 ? 0 : page - 1;

			StringBuilder sb = new StringBuilder();

			var commands = Enumerable.Skip<Command>(_pluginManager.Commands.Values, page * 7);
			int i = 0;
			foreach (var command in commands)
			{
				sb.Append(PluginManager.GetUsage(command));
				if (i++ >= 6) break;
				sb.Append("\n");
			}

			return new HelpResponseByPage()
			{
				Body = sb.ToString(),
				Page = page + 1,
				PageCount = (int) Math.Ceiling(_pluginManager.Commands.Count / 7f)
			};
		}
	}
}