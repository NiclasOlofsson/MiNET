using System;
using System.Collections.Generic;
using System.IO;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Inventory
{
	public static class InventoryUtils
	{
		public static List<Item> CreativeInventoryItems { get; } = new List<Item>();

		static InventoryUtils()
		{
			var creativeItems = ResourceUtil.ReadResource<List<CreativeItem>>("creativeitems.json", typeof(InventoryUtils), "Data");

			var uniqueId = 0;
			foreach (var itemData in creativeItems)
			{
				if (itemData.Id.StartsWith("minecraft:element")) continue;

				var item = ItemFactory.GetItem(itemData.Id, itemData.Metadata);
				if (item is ItemAir) continue;
				if (item is ItemBlock bb && bb.Block == null) continue;
				item.UniqueId = uniqueId++;

				if (itemData.BlockStates != null && item is ItemBlock itemBlock)
				{
					var bytes = Convert.FromBase64String(itemData.BlockStates);

					using MemoryStream memoryStream = new MemoryStream(bytes, 0, bytes.Length);
					var compound = Packet.ReadNbtCompound(memoryStream, false);

					var statesContainer = new BlockStateContainer();
					statesContainer.Id = itemData.Id;
					statesContainer.States = BlockFactory.GetBlockStates(compound);

					if (BlockFactory.BlockStates.TryGetValue(statesContainer, out var blockStateContainer))
					{
						itemBlock.Block.SetState(blockStateContainer);
						itemBlock.Metadata = blockStateContainer.Data;
					}
				}

				if (itemData.ExtraData != null)
				{
					var bytes = Convert.FromBase64String(itemData.ExtraData);

					using MemoryStream memoryStream = new MemoryStream(bytes, 0, bytes.Length);
					item.ExtraData = Packet.ReadNbtCompound(memoryStream, false);

				}

				CreativeInventoryItems.Add(item);
			}
		}

		public static CreativeItemStacks GetCreativeMetadataSlots()
		{
			var slotData = new CreativeItemStacks();
			for (int i = 0; i < CreativeInventoryItems.Count; i++)
				slotData.Add(CreativeInventoryItems[i]);

			return slotData;
		}

		public class CreativeItem
		{
			[JsonProperty("name")]
			public string Id { get; set; }

			[JsonProperty("meta")]
			public short Metadata { get; set; }

			[JsonProperty("block_states")]
			public string BlockStates { get; set; }

			[JsonProperty("nbt")]
			public string ExtraData { get; set; }
		}
	}
}