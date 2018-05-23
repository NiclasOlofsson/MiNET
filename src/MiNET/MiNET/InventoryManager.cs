using System.Collections.Generic;
using System.Linq;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Entities;
using MiNET.Entities.Vehicles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (InventoryManager));

		private static byte _inventoryId = 2;

		private readonly Level _level;
        private Dictionary<BlockCoordinates, Inventory> _cacheByBlocks = new Dictionary<BlockCoordinates, Inventory>();
        private Dictionary<long, Inventory> _cacheByEntities = new Dictionary<long, Inventory>();
        private object _inventorySync = new object();

        public InventoryManager(Level level)
		{
			_level = level;
		}

		public Inventory GetInventory(int inventoryId)
		{
			lock (_inventorySync)
			{
				return _cacheByBlocks.Values.FirstOrDefault(inventory => inventory.Id == inventoryId) ?? 
                    _cacheByEntities.Values.FirstOrDefault(inventory => inventory.Id == inventoryId);
            }
		}

        public Inventory GetInventoryByEntityId(long entityId)
        {
            lock (_inventorySync)
            {
                if (_cacheByEntities.ContainsKey(entityId))
                {
                    Inventory cacheByEntity = _cacheByEntities[entityId];
                    if (cacheByEntity != null)
                        return cacheByEntity;
                }

                Entity entity;
                if (!_level.TryGetEntity(entityId, out entity))
                {
                    if (Log.IsDebugEnabled) Log.Warn($"No entity found with id {entityId}");
                    return null;
                }

                Inventory inventory;
                if (entity is ChestMinecart)
                {
                    inventory = new Inventory(GetInventoryId(), entity, 27)
                    {
                        Type = 0,
                        WindowsId = 10
                    };
                }
                else
                {
                    if (Log.IsDebugEnabled) Log.Warn($"Entity did not have a matching inventory {entity}");
                    return null;
                }

                _cacheByEntities[entityId] = inventory;
                return inventory;
            }
        }

        public Inventory GetInventory(BlockCoordinates inventoryCoord)
		{
			lock (_inventorySync)
			{
				if (_cacheByBlocks.ContainsKey(inventoryCoord))
				{
					Inventory cachedInventory = _cacheByBlocks[inventoryCoord];
					if (cachedInventory != null) return cachedInventory;
				}

				BlockEntity blockEntity = _level.GetBlockEntity(inventoryCoord);

				if (blockEntity == null)
				{
					if(Log.IsDebugEnabled) Log.Warn($"No blockentity found at {inventoryCoord}");
					return null;
				}

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
					if (Log.IsDebugEnabled) Log.Warn($"Block entity did not have a matching inventory {blockEntity}");
					return null;
				}

                _cacheByBlocks[inventoryCoord] = inventory;

				return inventory;
			}
		}

		private byte GetInventoryId()
		{
			lock (_inventorySync)
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