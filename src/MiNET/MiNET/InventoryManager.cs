using System.Collections.Generic;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private readonly Level _level;
		private Dictionary<BlockCoordinates, Inventory> _cache = new Dictionary<BlockCoordinates, Inventory>();


		public InventoryManager(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(BlockCoordinates inventoryCoord)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(inventoryCoord))
				{
					return _cache[inventoryCoord];
				}

				BlockEntity blockEntity = _level.GetBlockEntity(inventoryCoord);

				if (blockEntity == null) return null;

				NbtCompound comp = blockEntity.GetCompound();

				Inventory inventory = null;
				if (blockEntity is ChestBlockEntity)
				{
					inventory = new Inventory(blockEntity, 27, (NbtList) comp["Items"])
					{
						Id = 10,
						Type = 0,
						Size = 27,
					};
				}

				else if (blockEntity is FurnaceBlockEntity)
				{
					inventory = new Inventory(blockEntity, 3, (NbtList) comp["Items"])
					{
						Id = 10,
						Type = 2,
						Size = 3,
					};

					FurnaceBlockEntity furnace = (FurnaceBlockEntity) blockEntity;
					furnace.Inventory = inventory;
				}

				_cache[inventoryCoord] = inventory;

				return inventory;
			}
		}
	}
}