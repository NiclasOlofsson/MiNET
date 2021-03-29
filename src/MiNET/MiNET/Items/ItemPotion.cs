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

using log4net;
using MiNET.Effects;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemPotion : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemPotion));

		public ItemPotion(short metadata) : base("minecraft:potion", 373, metadata)
		{
		}

		private bool _isUsing;

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			if (_isUsing)
			{
				Consume(player);

				if (player.GameMode == GameMode.Survival || player.GameMode == GameMode.Adventure)
				{
					player.Inventory.ClearInventorySlot((byte) player.Inventory.InHandSlot);
					player.Inventory.SetFirstEmptySlot(ItemFactory.GetItem(374), true);
				}
				_isUsing = false;
				return;
			}

			_isUsing = true;
		}

		public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			_isUsing = false;
		}


		public virtual void Consume(Player player)
		{
			Effect e = null;
			switch (Metadata)
			{
				case 5:
					e = new NightVision
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 6:
					e = new NightVision
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 7:
					e = new Invisibility
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 8:
					e = new Invisibility
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 9:
					e = new JumpBoost
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 10:
					e = new JumpBoost
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 11:
					e = new JumpBoost
					{
						Duration = 1800,
						Level = 1
					};
					break;
				case 12:
					e = new FireResistance
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 13:
					e = new FireResistance
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 14:
					e = new Speed
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 15:
					e = new Speed
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 16:
					e = new Speed
					{
						Duration = 1800,
						Level = 1
					};
					break;
				case 17:
					e = new Slowness
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 18:
					e = new Slowness
					{
						Duration = 4800,
						Level = 0
					};
					break;
				case 19:
					e = new WaterBreathing
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 20:
					e = new WaterBreathing
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 21:
					e = new InstantHealth
					{
						Duration = 0,
						Level = 0
					};
					break;
				case 22:
					e = new InstantHealth
					{
						Duration = 0,
						Level = 1
					};
					break;
				case 23:
					e = new InstantDamage
					{
						Duration = 0,
						Level = 0
					};
					break;
				case 24:
					e = new InstantDamage
					{
						Duration = 0,
						Level = 1
					};
					break;
				case 25:
					e = new Poison
					{
						Duration = 900,
						Level = 0
					};
					break;
				case 26:
					e = new Poison
					{
						Duration = 2400,
						Level = 0
					};
					break;
				case 27:
					e = new Poison
					{
						Duration = 440,
						Level = 1
					};
					break;
				case 28:
					e = new Regeneration
					{
						Duration = 900,
						Level = 0
					};
					break;
				case 29:
					e = new Regeneration
					{
						Duration = 2400,
						Level = 0
					};
					break;
				case 30:
					e = new Regeneration
					{
						Duration = 440,
						Level = 1
					};
					break;
				case 31:
					e = new Strength
					{
						Duration = 3600,
						Level = 0
					};
					break;
				case 32:
					e = new Strength
					{
						Duration = 9600,
						Level = 0
					};
					break;
				case 33:
					e = new Strength
					{
						Duration = 1800,
						Level = 1
					};
					break;
				case 34:
					e = new Weakness
					{
						Duration = 1800,
						Level = 0
					};
					break;
				case 35:
					e = new Weakness
					{
						Duration = 4800,
						Level = 0
					};
					break;
			}

			if (e != null)
			{
				player.SetEffect(e);
			}
		}
	}
}