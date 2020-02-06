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
using System.Linq;
using System.Numerics;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
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
		public short Id { get; protected set; }
		public short Metadata { get; set; }
		public byte Count { get; set; }
		public virtual NbtCompound ExtraData { get; set; }

		[JsonIgnore] public ItemMaterial ItemMaterial { get; set; } = ItemMaterial.None;

		[JsonIgnore] public ItemType ItemType { get; set; } = ItemType.Item;

		[JsonIgnore] public int MaxStackSize { get; set; } = 64;

		[JsonIgnore] public bool IsStackable => MaxStackSize > 1;

		[JsonIgnore] public int Durability { get; set; }

		[JsonIgnore] public int FuelEfficiency { get; set; }

		protected internal Item(short id, short metadata = 0, int count = 1)
		{
			Id = id;
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

		public virtual void Release(Level world, Player player, BlockCoordinates blockCoordinates, long timeUsed)
		{
		}

		protected bool Equals(Item other)
		{
			if (Id != other.Id || Metadata != other.Metadata) return false;
			if (ExtraData == null ^ other.ExtraData == null) return false;

			byte[] saveToBuffer = null;
			if (other.ExtraData != null)
			{
				other.ExtraData.Name = string.Empty;
				saveToBuffer = new NbtFile(other.ExtraData).SaveToBuffer(NbtCompression.None);
			}

			byte[] saveToBuffer2 = null;
			if (ExtraData != null)
			{
				ExtraData.Name = string.Empty;
				saveToBuffer2 = new NbtFile(ExtraData).SaveToBuffer(NbtCompression.None);
			}
			var nbtCheck = !(saveToBuffer == null ^ saveToBuffer2 == null);
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
			unchecked
			{
				return (Id * 397) ^ Metadata.GetHashCode();
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return $"{GetType().Name}(Id={Id}, Meta={Metadata}) Count={Count}, NBT={ExtraData}";
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
	}

	public enum ItemType
	{
		//Tools
		Sword,
		Shovel,
		PickAxe,
		Axe,
		Item,
		Hoe,

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
}