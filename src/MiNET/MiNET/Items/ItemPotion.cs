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

		public ItemPotion() : base("minecraft:potion")
		{
		}

		public ItemPotion(short metadata) : base("minecraft:potion", metadata)
		{
		}

		// The Bedrock potion table for this protocol, metadata 0..46. 0..4 are the effectless
		// base potions (water, mundane, long mundane, thick, awkward). Durations are ticks.
		public static Effect[] GetEffects(short metadata)
		{
			return metadata switch
			{
				5 => new Effect[] {new NightVision {Duration = 3600, Level = 0}},
				6 => new Effect[] {new NightVision {Duration = 9600, Level = 0}},
				7 => new Effect[] {new Invisibility {Duration = 3600, Level = 0}},
				8 => new Effect[] {new Invisibility {Duration = 9600, Level = 0}},
				9 => new Effect[] {new JumpBoost {Duration = 3600, Level = 0}},
				10 => new Effect[] {new JumpBoost {Duration = 9600, Level = 0}},
				11 => new Effect[] {new JumpBoost {Duration = 1800, Level = 1}},
				12 => new Effect[] {new FireResistance {Duration = 3600, Level = 0}},
				13 => new Effect[] {new FireResistance {Duration = 9600, Level = 0}},
				14 => new Effect[] {new Speed {Duration = 3600, Level = 0}},
				15 => new Effect[] {new Speed {Duration = 9600, Level = 0}},
				16 => new Effect[] {new Speed {Duration = 1800, Level = 1}},
				17 => new Effect[] {new Slowness {Duration = 1800, Level = 0}},
				18 => new Effect[] {new Slowness {Duration = 4800, Level = 0}},
				19 => new Effect[] {new WaterBreathing {Duration = 3600, Level = 0}},
				20 => new Effect[] {new WaterBreathing {Duration = 9600, Level = 0}},
				21 => new Effect[] {new InstantHealth {Duration = 0, Level = 0}},
				22 => new Effect[] {new InstantHealth {Duration = 0, Level = 1}},
				23 => new Effect[] {new InstantDamage {Duration = 0, Level = 0}},
				24 => new Effect[] {new InstantDamage {Duration = 0, Level = 1}},
				25 => new Effect[] {new Poison {Duration = 900, Level = 0}},
				26 => new Effect[] {new Poison {Duration = 2400, Level = 0}},
				27 => new Effect[] {new Poison {Duration = 440, Level = 1}},
				28 => new Effect[] {new Regeneration {Duration = 900, Level = 0}},
				29 => new Effect[] {new Regeneration {Duration = 2400, Level = 0}},
				30 => new Effect[] {new Regeneration {Duration = 440, Level = 1}},
				31 => new Effect[] {new Strength {Duration = 3600, Level = 0}},
				32 => new Effect[] {new Strength {Duration = 9600, Level = 0}},
				33 => new Effect[] {new Strength {Duration = 1800, Level = 1}},
				34 => new Effect[] {new Weakness {Duration = 1800, Level = 0}},
				35 => new Effect[] {new Weakness {Duration = 4800, Level = 0}},
				36 => new Effect[] {new Wither {Duration = 800, Level = 1}},
				37 => new Effect[] {new Slowness {Duration = 400, Level = 3}, new Resistance {Duration = 400, Level = 2}},
				38 => new Effect[] {new Slowness {Duration = 800, Level = 3}, new Resistance {Duration = 800, Level = 2}},
				39 => new Effect[] {new Slowness {Duration = 400, Level = 5}, new Resistance {Duration = 400, Level = 3}},
				40 => new Effect[] {new SlowFalling {Duration = 1800, Level = 0}},
				41 => new Effect[] {new SlowFalling {Duration = 4800, Level = 0}},
				42 => new Effect[] {new Slowness {Duration = 400, Level = 3}},
				43 => new Effect[] {new WindCharged {Duration = 3600, Level = 0}},
				44 => new Effect[] {new Weaving {Duration = 3600, Level = 0}},
				45 => new Effect[] {new Oozing {Duration = 3600, Level = 0}},
				46 => new Effect[] {new Infested {Duration = 3600, Level = 0}},
				_ => new Effect[0]
			};
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			CompleteUsing(world, player);

			player.ItemUseStartedAt = DateTime.UtcNow;
		}

		public override void CompleteUsing(Level world, Player player)
		{
			if (!player.HasUsedItemFor(32)) return;
			player.ItemUseStartedAt = null;

			Consume(player);

			if (player.GameMode == GameMode.Survival || player.GameMode == GameMode.Adventure)
			{
				player.Inventory.ClearInventorySlot((byte) player.Inventory.InHandSlot);
				player.Inventory.SetFirstEmptySlot(ItemFactory.GetItemByName("minecraft:glass_bottle"), true);
			}
		}


		public virtual void Consume(Player player)
		{
			foreach (Effect effect in GetEffects(Metadata))
			{
				player.SetEffect(effect);
			}
		}
	}
}