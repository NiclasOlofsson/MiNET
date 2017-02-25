using System.Collections.Generic;
using System.Linq;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (InventoryManager));

		private static byte _inventoryId = 2;

		private readonly Level _level;
		private Dictionary<BlockCoordinates, Inventory> _cache = new Dictionary<BlockCoordinates, Inventory>();


		public InventoryManager(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(int inventoryId)
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
					inventory = new Inventory(GetInventoryId(), blockEntity, 27, (NbtList) comp["Items"])
					{
						Type = 0,
						WindowsId = 10,
					};
				}
				else if (blockEntity is EnchantingTableBlockEntity)
				{
					inventory = new Inventory(GetInventoryId(), blockEntity, 2, (NbtList)comp["Items"])
					{
						Type = 3,
						WindowsId = 12,
					};
				}
				else if (blockEntity is FurnaceBlockEntity)
				{
					inventory = new Inventory(GetInventoryId(), blockEntity, 3, (NbtList) comp["Items"])
					{
						Type = 2,
						WindowsId = 11,
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

		private byte GetInventoryId()
		{
			lock (_cache)
			{
				_inventoryId++;
				if (_inventoryId == 0x78)
					_inventoryId++;
				if (_inventoryId == 0x79)
					_inventoryId++;

				return _inventoryId;
			}
		}
	}
}