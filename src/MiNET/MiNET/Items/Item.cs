using System.Numerics;
using fNbt;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	/// <summary>
	///     Items are objects which only exist within the player's inventory and hands - which means, they cannot be placed in
	///     the game world. Some items simply place blocks or entities into the game world when used. They are thus an item
	///     when in the inventory and a block when placed. Some examples of objects which exhibit these properties are item
	///     frames, which turn into an entity when placed, and beds, which turn into a group of blocks when placed. When
	///     equipped, items (and blocks) briefly display their names above the HUD.
	/// </summary>
	public class Item
	{
		public short Id { get; protected set; }
		public short Metadata { get; set; }
		public byte Count { get; set; }
		public virtual NbtCompound ExtraData { get; set; }

		public ItemMaterial ItemMaterial { get; set; } = ItemMaterial.None;
		public ItemType ItemType { get; set; } = ItemType.Item;
		public int MaxStackSize { get; set; } = 64;
		public bool IsStackable => MaxStackSize > 1;
		public int Durability { get; set; }
		public int FuelEfficiency { get; set; }

		protected internal Item(short id, short metadata = 0, int count = 1)
		{
			Id = id;
			Metadata = metadata;
			Count = (byte) count;
		}

		public virtual void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
		}

		public virtual void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
		}

		public virtual bool BreakBlock(Level world, Player player, Block block, BlockEntity blockEntity)
		{
			return true;
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
				case BlockFace.East:
					return target + Level.East;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.North:
					return target + Level.North;
				case BlockFace.South:
					return target + Level.South;
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
			return Id == other.Id && Metadata == other.Metadata;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Item) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Id*397) ^ Metadata.GetHashCode();
			}
		}

		public override string ToString()
		{
			return $"Type={GetType().Name}, Id={Id}, Metadata={Metadata}, Count={Count}, ExtraData={ExtraData}";
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
}