using System.Collections.Generic;
using System.Linq;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private static byte _inventoryId = 15;

		private readonly Level _level;
		private Dictionary<BlockCoordinates, Inventory> _cache = new Dictionary<BlockCoordinates, Inventory>();


		public InventoryManager(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(byte inventoryId)
		{
			lock (_cache)
			{
				return _cache.Values.FirstOrDefault(inventory => inventory.Id == inventoryId);
			}
		}

		public Inventory GetInventory(BlockCoordinates inventoryCoord)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(inventoryCoord))
				{
					Inventory cachedInventory = _cache[inventoryCoord];
					if (cachedInventory != null) return cachedInventory;
				}

				BlockEntity blockEntity = _level.GetBlockEntity(inventoryCoord);

				if (blockEntity == null) return null;

				NbtCompound comp = blockEntity.GetCompound();

				Inventory inventory;
				if (blockEntity is ChestBlockEntity)
				{
					inventory = new Inventory(_inventoryId++, blockEntity, 27, (NbtList) comp["Items"])
					{
						Type = 0,
					};
				}
				else if (blockEntity is FurnaceBlockEntity)
				{
					inventory = new Inventory(_inventoryId++, blockEntity, 3, (NbtList) comp["Items"])
					{
						Type = 2,
					};

					FurnaceBlockEntity furnace = (FurnaceBlockEntity) blockEntity;
					furnace.Inventory = inventory;
				}
				else
				{
					return null;
				}

				_cache[inventoryCoord] = inventory;

				return inventory;
			}
		}
	}
}