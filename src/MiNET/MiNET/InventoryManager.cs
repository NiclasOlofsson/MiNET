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

				ChestBlockEntity blockEntity = _level.GetBlockEntity(inventoryCoord) as ChestBlockEntity;

				if (blockEntity == null) return null;

				NbtCompound comp = blockEntity.GetCompound();

				Inventory inventory = new Inventory(blockEntity, (NbtList) comp["Items"])
				{
					Id = 10,
					Type = 0,
					Size = 27,
				};

				_cache[inventoryCoord] = inventory;

				return inventory;
			}
		}
	}
}