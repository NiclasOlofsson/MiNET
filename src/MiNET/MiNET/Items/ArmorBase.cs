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

using System;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Utils;
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

		protected ArmorBase(ArmorType armorType, string name, short id, short metadata = 0, int count = 1) : base(name, id, metadata, count)
		{
			ArmorType = armorType;
		}

		protected void SwithItem(Player player, Item item)
		{
			byte slot = (byte) player.Inventory.Slots.IndexOf(this);
			player.Inventory.SetInventorySlot(slot, item);

			UniqueId = Environment.TickCount;
			player.Inventory.SetArmorSlot(ArmorType, this, false);
		}

		public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
		{
			return ++Metadata >= Durability;
		}
	}

	public abstract class ArmorHelmetBase : ArmorBase
	{
		protected ArmorHelmetBase(string name, short id, short metadata = 0, int count = 1) : base(ArmorType.Helmet, name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			SwithItem(player, player.Inventory.Helmet);
		}
	}

	public abstract class ArmorChestplateBase : ArmorBase
	{
		protected ArmorChestplateBase(string name, short id, short metadata = 0, int count = 1) : base(ArmorType.Chestplate, name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			SwithItem(player, player.Inventory.Chest);
		}
	}

	public abstract class ArmorLeggingsBase : ArmorBase
	{
		protected ArmorLeggingsBase(string name, short id, short metadata = 0, int count = 1) : base(ArmorType.Leggings, name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			SwithItem(player, player.Inventory.Leggings);
		}
	}

	public abstract class ArmorBootsBase : ArmorBase
	{
		protected ArmorBootsBase(string name, short id, short metadata = 0, int count = 1) : base(ArmorType.Boots, name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			SwithItem(player, player.Inventory.Boots);
		}
	}
}