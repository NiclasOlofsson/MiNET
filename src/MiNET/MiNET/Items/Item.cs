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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using Newtonsoft.Json;

namespace MiNET.Items
{
	/// <summary>
	///     Items are objects which only exist within the player's inventory and hands - which means, they cannot be placed in
	///     the game world. Some items simply place blocks or entities into the game world when used. They are thus an item
	///     when in the inventory and a block when placed. Some examples of objects which exhibit these properties are item
	///     frames, which turn into an entity when placed, and beds, which turn into a group of blocks when placed. When
	///     equipped, items (and blocks) briefly display their names above the HUD.
	/// </summary>
	public class Item : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Item));

		public int UniqueId { get; set; } = Environment.TickCount;

		/// <summary>
		///     The registry string id ("minecraft:apple"). This is the item's identity: it is what the
		///     save format stores, what recipe and creative data reference, and the only id that means
		///     the same thing across game versions. <see cref="NetworkId" /> is derived from it.
		/// </summary>
		public string Name { get; protected internal set; } = string.Empty;

		public int NetworkId { get; set; } = -1;

		/// <summary>
		///     Nothing in this slot. Air has a registry entry because it is a block, but an empty
		///     stack goes on the wire as network id 0, which no real item uses. This is the check that
		///     used to be written "Id == 0".
		/// </summary>
		[JsonIgnore]
		public bool IsAir => string.IsNullOrEmpty(Name) || Name.Equals("minecraft:air", StringComparison.OrdinalIgnoreCase);

		/// <summary>
		///     The raw "metadata" varint as read off the wire alongside <see cref="NetworkId" /> (ReadNetworkItemStackDescriptor/
		///     ReadItem/ReadNetworkItemStackDescriptor). -1 means unknown (item was constructed server-side, not decoded), so
		///     the writer must not assume it's 0 and instead re-derive it via <see cref="MiNET.Net.Items.ItemTranslator" />.
		///     Only set together with NetworkId, and only from a decode path, so the two stay in sync as the
		///     "we know the exact wire encoding" marker.
		/// </summary>
		public int NetworkMetadata { get; set; } = -1;

		public int RuntimeId { get; set; }
		public short Metadata { get; set; }
		public byte Count { get; set; }
		public virtual NbtCompound ExtraData { get; set; }

		/// <summary>
		///     Block names this item can be placed on / can destroy while in adventure mode ("extra data"
		///     of the item stack descriptor). Null when the wire didn't carry the corresponding list.
		/// </summary>
		public List<string> CanPlaceOn { get; set; }

		public List<string> CanDestroy { get; set; }

		/// <summary>
		///     The shield's "blocking_tick" trailer in the item extra-data blob. Only present when this
		///     item is a shield (see ReadItemExtraData/WriteItemExtraData's includeBlockingTick).
		/// </summary>
		public long BlockingTick { get; set; }

		/// <summary>
		///     Set only when this Item stands in for a recipe ingredient descriptor variant that isn't a
		///     plain (id, meta) item - molang expression, item tag, string id+meta, or complex alias (see
		///     Packet.ReadRecipeIngredient/WriteRecipeIngredient). Null for every ordinary item, including
		///     ordinary recipe ingredients (the common "int_id_meta" wire variant).
		/// </summary>
		public RecipeIngredientDescriptor IngredientDescriptor { get; set; }

		[JsonIgnore] public ItemMaterial ItemMaterial { get; set; } = ItemMaterial.None;

		[JsonIgnore] public ItemType ItemType { get; set; } = ItemType.Item;

		[JsonIgnore] public int MaxStackSize { get; set; } = 64;

		[JsonIgnore] public bool IsStackable => MaxStackSize > 1;

		[JsonIgnore] public int Durability { get; set; }

		[JsonIgnore] public int FuelEfficiency { get; set; }

		protected internal Item(string name, short metadata = 0, int count = 1)
		{
			Name = name;
			Metadata = metadata;
			Count = (byte) count;
		}

		public virtual void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
		}

		public virtual void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
		}

		public virtual bool BreakBlock(Level world, Player player, Block block, BlockEntity blockEntity)
		{
			return true;
		}

		public virtual bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
		{
			return false;
		}

		protected virtual int GetMaxUses()
		{
			switch (ItemMaterial)
			{
				case ItemMaterial.Wood:
					return 60;
				case ItemMaterial.Gold:
					return 33;
				case ItemMaterial.Stone:
					return 132;
				case ItemMaterial.Iron:
					return 251;
				case ItemMaterial.Diamond:
					return 1562;
				default:
					return 0;
			}
		}

		public virtual bool Animate(Level world, Player player)
		{
			return false;
		}

		public BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
		{
			switch (face)
			{
				case BlockFace.Down:
					return target + Level.Down;
				case BlockFace.Up:
					return target + Level.Up;
				case BlockFace.North:
					return target + Level.North;
				case BlockFace.South:
					return target + Level.South;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.East:
					return target + Level.East;
				default:
					return target;
			}
		}

		public int GetDamage()
		{
			switch (ItemType)
			{
				case ItemType.Sword:
					return GetSwordDamage(ItemMaterial);
				case ItemType.Item:
					return 1;
				case ItemType.Axe:
					return GetAxeDamage(ItemMaterial);
				case ItemType.PickAxe:
					return GetPickAxeDamage(ItemMaterial);
				case ItemType.Shovel:
					return GetShovelDamage(ItemMaterial);
				default:
					return 1;
			}
		}

		protected int GetSwordDamage(ItemMaterial itemMaterial)
		{
			switch (itemMaterial)
			{
				case ItemMaterial.Wood:
					return 5;
				case ItemMaterial.Gold:
					return 5;
				case ItemMaterial.Stone:
					return 6;
				case ItemMaterial.Iron:
					return 7;
				case ItemMaterial.Diamond:
					return 8;
				default:
					return 1;
			}
		}

		private int GetAxeDamage(ItemMaterial itemMaterial)
		{
			return GetSwordDamage(itemMaterial) - 1;
		}

		private int GetPickAxeDamage(ItemMaterial itemMaterial)
		{
			return GetSwordDamage(itemMaterial) - 2;
		}

		private int GetShovelDamage(ItemMaterial itemMaterial)
		{
			return GetSwordDamage(itemMaterial) - 3;
		}

		public virtual Item GetSmelt()
		{
			return null;
		}

		public virtual void Release(Level world, Player player, BlockCoordinates blockCoordinates)
		{
		}

		public virtual void CompleteUsing(Level world, Player player)
		{
		}

		protected bool Equals(Item other)
		{
			if (!string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) || Metadata != other.Metadata) return false;
			if (ExtraData == null ^ other.ExtraData == null) return false;

			//TODO: This doesn't work in  most cases. We need to fix comparison when name == null
			byte[] saveToBuffer = null;
			if(other.ExtraData?.Name != null) saveToBuffer = new NbtFile(other.ExtraData).SaveToBuffer(NbtCompression.None);
			byte[] saveToBuffer2 = null;
			if(ExtraData?.Name != null) saveToBuffer2 = new NbtFile(ExtraData).SaveToBuffer(NbtCompression.None);
			bool nbtCheck = !(saveToBuffer == null ^ saveToBuffer2 == null);
			if (nbtCheck)
			{
				if (saveToBuffer == null)
				{
					nbtCheck = true;
				}
				else
				{
					nbtCheck = saveToBuffer.SequenceEqual(saveToBuffer2);
				}
			}
			return nbtCheck;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (!(obj is Item)) return false;
			return Equals((Item) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Name), Metadata);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return $"{GetType().Name}(Name={Name}, Meta={Metadata}, UniqueId={UniqueId}) Count={Count}, NBT={ExtraData}";
		}

		public bool Interact(Level level, Player player, Entity target)
		{
			return false; // Not handled
		}
	}

	public enum ItemMaterial
	{
		//Armor Only
		Leather = -2,
		Chain = -1,

		None = 0,
		Wood = 1,
		Stone = 2,
		Gold = 3,
		Iron = 4,
		Diamond = 5,
		Netherite = 6
	}

	public enum ItemType
	{
		//Tools
		Sword,
		Bow,
		Shovel,
		PickAxe,
		Axe,
		Item,
		Hoe,
		Sheers,
		FlintAndSteel,
		Elytra,
		Trident,
		CarrotOnAStick,
		FishingRod,
		Book,

		//Armor
		Helmet,
		Chestplate,
		Leggings,
		Boots
	}

	public enum ItemDamageReason
	{
		BlockBreak,
		BlockInteract,
		EntityAttack,
		EntityInteract,
		ItemUse,
	}

	/// <summary>
	///     Which variant of the RecipeIngredient wire union (Packet.ReadRecipeIngredient) an item stands
	///     for: a molang expression, an item tag, a string id+meta pair, a complex alias name - or, for a
	///     recipe built from MiNET's own recipe registry, the plain int_id_meta variant carrying the
	///     registry string id the writer resolves the wire network id from. See
	///     <see cref="Item.IngredientDescriptor" />.
	/// </summary>
	public class RecipeIngredientDescriptor
	{
		/// <summary>1 = int_id_meta (by <see cref="Name" />), 2 = molang, 3 = item_tag, 4 = string_id_meta, 5 = complex_alias.</summary>
		public byte Type { get; set; }

		/// <summary>Molang expression (type 2) / tag (type 3) / item name (type 4) / alias name (type 5).</summary>
		public string Text { get; set; }

		/// <summary>
		///     Registry string id ("minecraft:stick") for type 1 - the durable identity the writer resolves
		///     the wire network id from. Only set for ingredients built from recipe data (recipes.json or a
		///     plugin); a decoded int_id_meta ingredient carries no descriptor and re-emits the id it read.
		/// </summary>
		public string Name { get; set; }

		/// <summary>Molang version byte (type 2 only).</summary>
		public byte MolangVersion { get; set; }

		/// <summary>Metadata (type 1 and type 4).</summary>
		public short Metadata { get; set; }
	}
}