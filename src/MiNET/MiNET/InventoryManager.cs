#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Linq;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET
{
	public class InventoryManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryManager));

		private static byte _inventoryId = 2;

		private readonly Level _level;
		private Dictionary<BlockCoordinates, Inventory> _cache = new Dictionary<BlockCoordinates, Inventory>();


		public InventoryManager(Level level)
		{
			_level = level;
		}

		public virtual Inventory GetInventory(int inventoryId)
		{
			lock (_cache)
			{
				return _cache.Values.FirstOrDefault(inventory => inventory.Id == inventoryId);
			}
		}

		public virtual Inventory GetInventory(BlockCoordinates inventoryCoord)
		{
			lock (_cache)
			{
				if (_cache.ContainsKey(inventoryCoord))
				{
					Inventory cachedInventory = _cache[inventoryCoord];
					if (cachedInventory != null) return cachedInventory;
				}

				BlockEntity blockEntity = _level.GetBlockEntity(inventoryCoord);
				if (blockEntity == null)
				{
					Log.Warn($"Found no block entity");
					Block inventoryBlock = _level.GetBlock(inventoryCoord);
					switch (inventoryBlock)
					{
						case Chest _:
							blockEntity = new ChestBlockEntity();
							break;
						case ShulkerBox _:
							blockEntity = new ShulkerBoxBlockEntity();
							break;
					}
				}

				if (blockEntity == null)
				{
					if (Log.IsDebugEnabled) Log.Debug($"No blockentity found at {inventoryCoord}");
					return null;
				}

				NbtCompound comp = blockEntity.GetCompound();
				if (Log.IsDebugEnabled) Log.Warn($"Found block entity at {inventoryCoord}\n{comp}");


				Inventory inventory;
				switch (blockEntity)
				{
					case ChestBlockEntity _:
					case ShulkerBoxBlockEntity _:
						inventory = new Inventory(GetInventoryId(), blockEntity, 27, (NbtList) comp["Items"])
						{
							Type = 0,
							WindowsId = 10,
						};
						break;
					case EnchantingTableBlockEntity _:
						inventory = new Inventory(GetInventoryId(), blockEntity, 2, (NbtList) comp["Items"])
						{
							Type = 3,
							WindowsId = 12,
						};
						break;
					case FurnaceBlockEntity furnaceBlockEntity:
					{
						inventory = new Inventory(GetInventoryId(), furnaceBlockEntity, 3, (NbtList) comp["Items"])
						{
							Type = 2,
							WindowsId = 11,
						};

						furnaceBlockEntity.Inventory = inventory;
						break;
					}
					case BlastFurnaceBlockEntity furnaceBlockEntity:
					{
						inventory = new Inventory(GetInventoryId(), furnaceBlockEntity, 3, (NbtList) comp["Items"])
						{
							Type = 27,
							WindowsId = 13,
						};

						furnaceBlockEntity.Inventory = inventory;
						break;
					}
					default:
					{
						if (Log.IsDebugEnabled) Log.Warn($"Block entity did not have a matching inventory {blockEntity}");
						return null;
					}
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