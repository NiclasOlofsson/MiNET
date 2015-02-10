using System.Collections.Generic;
using Craft.Net.Common;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private readonly Level _level;
		private Dictionary<Coordinates3D, Inventory> _cache = new Dictionary<Coordinates3D, Inventory>();


		public InventoryManager(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(Coordinates3D inventoryCoord)
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