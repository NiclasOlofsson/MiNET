using System.Collections.Generic;
using MiNET.Items;
using MiNET.Plugins.Attributes;

namespace MiNET.Plugins.Commands
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

		[Command(Name = "op")]
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

		[Command]
		public SimpleResponse Give(Player commander, Target player, ItemTypeEnum itemName, int amount = 1, int data = 0)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);

					Item item = ItemFactory.GetItem(ItemFactory.GetItemIdByName(itemName.Value), (short)data, (byte)amount);

					var inventory = p.Inventory.SetFirstEmptySlot(item, true, false);
				}
				body = string.Join(", ", names);
			}


			return new SimpleResponse { Body = $"Gave {body} {amount} of {itemName.Value}." };
		}

	}
}