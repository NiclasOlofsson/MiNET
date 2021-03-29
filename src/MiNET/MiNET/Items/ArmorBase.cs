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
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public abstract class ArmorHelmetBase : Item
	{
		protected ArmorHelmetBase(string name, short id, short metadata = 0, int count = 1) : base(name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			byte slot = (byte) player.Inventory.Slots.IndexOf(this);
			player.Inventory.SetInventorySlot(slot, player.Inventory.Helmet);

			UniqueId = Environment.TickCount;
			player.Inventory.Helmet = this;
			player.SendArmorForPlayer();
		}
	}

	public abstract class ArmorChestplateBase : Item
	{
		protected ArmorChestplateBase(string name, short id, short metadata = 0, int count = 1) : base(name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			byte slot = (byte) player.Inventory.Slots.IndexOf(this);
			player.Inventory.SetInventorySlot(slot, player.Inventory.Chest);

			UniqueId = Environment.TickCount;
			player.Inventory.Chest = this;
			player.SendArmorForPlayer();
		}
	}

	public abstract class ArmorLeggingsBase : Item
	{
		protected ArmorLeggingsBase(string name, short id, short metadata = 0, int count = 1) : base(name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			byte slot = (byte) player.Inventory.Slots.IndexOf(this);
			player.Inventory.SetInventorySlot(slot, player.Inventory.Leggings);

			UniqueId = Environment.TickCount;
			player.Inventory.Leggings = this;
			player.SendArmorForPlayer();
		}
	}

	public abstract class ArmorBootsBase : Item
	{
		protected ArmorBootsBase(string name, short id, short metadata = 0, int count = 1) : base(name, id, metadata, count)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			byte slot = (byte) player.Inventory.Slots.IndexOf(this);
			player.Inventory.SetInventorySlot(slot, player.Inventory.Boots);

			UniqueId = Environment.TickCount;
			player.Inventory.Boots = this;
			player.SendArmorForPlayer();
		}
	}
}