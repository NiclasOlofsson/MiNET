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
			var creativeItems = ResourceUtil.ReadResource<List<ExternalDataItem>>("creativeitems.json", typeof(InventoryUtils), "Data");

			var uniqueId = 0;
			foreach (var itemData in creativeItems)
			{
				if (TryGetItemFromExternalData(itemData, out var item))
				{
					item.UniqueId = uniqueId++;
					CreativeInventoryItems.Add(item);
				}
			}
		}

		public static CreativeItemStacks GetCreativeMetadataSlots()
		{
			var slotData = new CreativeItemStacks();
			for (int i = 0; i < CreativeInventoryItems.Count; i++)
				slotData.Add(CreativeInventoryItems[i]);

			return slotData;
		}

		public static bool TryGetItemFromExternalData(ExternalDataItem itemData, out Item result)
		{
			result = null;

			if (string.IsNullOrEmpty(itemData.Id)) return false;
			if (itemData.Id.StartsWith("minecraft:element")) return false;

			var item = ItemFactory.GetItem(itemData.Id, itemData.Metadata, (byte) itemData.Count);
			if (item is ItemAir) return false;

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

			result = item;
			return true;
		}
	}
}