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

using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public enum ArmorType
	{
		Helmet,
		Chestplate,
		Leggings,
		Boots
	}

	public abstract class ArmorBase : Item
	{
		protected ArmorType ArmorType { get; set; }

		protected ArmorBase(ArmorType armorType) : base()
		{
			ArmorType = armorType;

			MaxStackSize = 1;
			Durability = CalculateDurability();
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			SwithItem(player);
		}

		public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
		{
			return ++Metadata >= Durability;
		}

		private int CalculateDurability()
		{
			var armor = ArmorType switch
			{
				ArmorType.Helmet => 11,
				ArmorType.Chestplate => 16,
				ArmorType.Leggings => 15,
				ArmorType.Boots => 13,
				_ => 0
			};

			var material = ItemMaterial switch
			{
				ItemMaterial.Leather => 5,
				ItemMaterial.Gold => 7,
				ItemMaterial.Chain => 15,
				ItemMaterial.Iron => 15,
				ItemMaterial.Turtle => 25,
				ItemMaterial.Diamond => 33,
				ItemMaterial.Netherite => 37,
				_ => 0
			};

			return armor * material;
		}

		private void SwithItem(Player player)
		{
			byte slot = (byte) player.Inventory.Slots.IndexOf(this);
			player.Inventory.SetInventorySlot(slot, player.Inventory.GetArmorSlot(ArmorType));

			UniqueId = GetUniqueId();
			player.Inventory.SetArmorSlot(ArmorType, this);

			PlayEquipSound(player);
		}

		private void PlayEquipSound(Player player)
		{
			var soundType = (ItemMaterial, ItemType) switch
			{
				(ItemMaterial.Leather, _) => LevelSoundEventType.ArmorEquipLeather,
				(ItemMaterial.Chain, _) => LevelSoundEventType.ArmorEquipChain,
				(ItemMaterial.Gold, _) => LevelSoundEventType.ArmorEquipGold,
				(ItemMaterial.Iron, _) => LevelSoundEventType.ArmorEquipIron,
				(ItemMaterial.Diamond, _) => LevelSoundEventType.ArmorEquipDiamond,
				(ItemMaterial.Netherite, _) => LevelSoundEventType.ArmorEquipNetherite,
				(_, ItemType.Elytra) => LevelSoundEventType.ArmorEquipElytra,
				_ => LevelSoundEventType.ArmorEquipGeneric
			};

			player.Level.BroadcastSound(player.GetEyesPosition(), soundType);
		}
	}

	public abstract class ItemArmorHelmetBase : ArmorBase
	{
		protected ItemArmorHelmetBase() : base(ArmorType.Helmet)
		{
		}
	}

	public abstract class ItemArmorChestplateBase : ArmorBase
	{
		protected ItemArmorChestplateBase() : base(ArmorType.Chestplate)
		{
		}
	}

	public abstract class ItemArmorLeggingsBase : ArmorBase
	{
		protected ItemArmorLeggingsBase() : base(ArmorType.Leggings)
		{
		}
	}

	public abstract class ItemArmorBootsBase : ArmorBase
	{
		protected ItemArmorBootsBase() : base(ArmorType.Boots)
		{
		}
	}
}