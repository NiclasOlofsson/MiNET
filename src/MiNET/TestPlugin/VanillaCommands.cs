using System.Collections.Generic;
using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace TestPlugin
{
	public class VanillaCommands
	{
		private readonly PluginManager _pluginManager;

		public VanillaCommands(PluginManager pluginManager)
		{
			_pluginManager = pluginManager;
		}

		public class SimpleResponse
		{
			public string Body { get; set; }
		}

		[Command(Command = "op")]
		public SimpleResponse MakeOperator(Player commander, Target player)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);
				}
				body = string.Join(", ", names);
			}
			else if (player.Entities != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Entities)
				{
					names.Add(p.NameTag ?? p.EntityId + "");
				}
				body = string.Join(", ", names);
			}

			return new SimpleResponse() {Body = $"Oped: {body}"};
		}

		[Command]
		public SimpleResponse SetBlock(Player commander, BlockPos position, BlockTypeEnum tileName, int tileData = 0)
		{
			return new SimpleResponse {Body = $"Set block complete. {position.XRelative} {tileName.Value}"};
		}
	}
}