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
using fNbt;
using log4net;
using MiNET.Effects;
using MiNET.Entities;
using MiNET.Entities.Projectiles;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class DamageCalculator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(DamageCalculator));

		public virtual double CalculateItemDamage(Player player, Item item, Player target)
		{
			double damage = item.GetDamage();

			var enchantings = item.GetEnchantings();

			double increase = 0;

			foreach (var enchanting in enchantings)
			{
				EnchantingType enchantingType = enchanting.Id;
				switch (enchantingType)
				{
					case EnchantingType.Sharpness:
						increase = 1 + (enchanting.Level * 0.5);
						break;
					case EnchantingType.Smite:
						break;
					case EnchantingType.BaneOfArthropods:
						break;
					case EnchantingType.Knockback:
						// Need to deal with for all knockbacks
						break;
					case EnchantingType.FireAspect:
						// Set target on fire. Need to deal with in "take hit" perhaps?
						break;
					case EnchantingType.Looting:
						break;
					case EnchantingType.Efficiency:
						break;
					case EnchantingType.SilkTouch:
						break;
					case EnchantingType.Unbreaking:
						break;
					case EnchantingType.Fortune:
						break;
					case EnchantingType.Power:
						break;
					case EnchantingType.Punch:
						break;
					case EnchantingType.Flame:
						break;
					case EnchantingType.Infinity:
						break;
					case EnchantingType.LuckOfTheSea:
						break;
					case EnchantingType.Lure:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}


			return damage + increase; //Item Damage.
		}

		public virtual double CalculateFallDamage(Player player, double damage, Player target)
		{
			var fallDamage = new Random().Next((int) (damage / 2 + 2));

			McpeAnimate animate = McpeAnimate.CreateObject();
			animate.runtimeEntityId = target.EntityId;
			animate.actionId = 4;
			player.Level.RelayBroadcast(animate);
			return fallDamage;
		}

		public virtual double CalculateEffectDamage(Player player, double damage, Player target)
		{
			double effectDamage = 0;
			Effect effect;
			if (player.Effects.TryGetValue(EffectType.Weakness, out effect))
			{
				effectDamage -= (effect.Level + 1) * 4;
			}
			else if (player.Effects.TryGetValue(EffectType.Strength, out effect))
			{
				effectDamage += (effect.Level + 1) * 3;
			}

			return effectDamage;
		}

		public virtual double CalculateDamageIncreaseFromEnchantments(Player player, Item tool, Player target)
		{
			if (tool == null) return 0;
			if (tool.ExtraData == null) return 0;

			NbtList enchantings;
			if (!tool.ExtraData.TryGet("ench", out enchantings)) return 0;

			double increase = 0;
			foreach (NbtCompound enchanting in enchantings)
			{
				short level = enchanting["lvl"].ShortValue;

				if (level == 0) continue;

				short id = enchanting["id"].ShortValue;
				if (id == 9)
				{
					increase += 1 + ((level - 1) * 0.5);
				}
			}

			return increase;
		}

		public virtual double CalculatePlayerDamage(Entity source, Player target, Item tool, double damage, DamageCause cause)
		{
			double originalDamage = damage;
			double armorValue = 0;
			double epfValue = 0;

			{
				{
					Item armorPiece = target.Inventory.Helmet;
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 1;
							break;
						case ItemMaterial.Gold:
							armorValue += 2;
							break;
						case ItemMaterial.Chain:
							armorValue += 2;
							break;
						case ItemMaterial.Iron:
							armorValue += 2;
							break;
						case ItemMaterial.Diamond:
							armorValue += 3;
							break;
					}
					epfValue += CalculateDamageReductionFromEnchantments(source, armorPiece, tool, cause);
				}

				{
					Item armorPiece = target.Inventory.Chest;
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 3;
							break;
						case ItemMaterial.Gold:
							armorValue += 5;
							break;
						case ItemMaterial.Chain:
							armorValue += 5;
							break;
						case ItemMaterial.Iron:
							armorValue += 6;
							break;
						case ItemMaterial.Diamond:
							armorValue += 8;
							break;
					}
					epfValue += CalculateDamageReductionFromEnchantments(source, armorPiece, tool, cause);
				}

				{
					Item armorPiece = target.Inventory.Leggings;
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 2;
							break;
						case ItemMaterial.Gold:
							armorValue += 3;
							break;
						case ItemMaterial.Chain:
							armorValue += 4;
							break;
						case ItemMaterial.Iron:
							armorValue += 5;
							break;
						case ItemMaterial.Diamond:
							armorValue += 6;
							break;
					}
					epfValue += CalculateDamageReductionFromEnchantments(source, armorPiece, tool, cause);
				}

				{
					Item armorPiece = target.Inventory.Boots;
					switch (armorPiece.ItemMaterial)
					{
						case ItemMaterial.Leather:
							armorValue += 1;
							break;
						case ItemMaterial.Gold:
							armorValue += 1;
							break;
						case ItemMaterial.Chain:
							armorValue += 1;
							break;
						case ItemMaterial.Iron:
							armorValue += 2;
							break;
						case ItemMaterial.Diamond:
							armorValue += 3;
							break;
					}
					epfValue += CalculateDamageReductionFromEnchantments(source, armorPiece, tool, cause);
				}
			}

			damage = damage * (1 - Math.Max(armorValue / 5, armorValue - damage / 2) / 25);

			epfValue = Math.Min(20, epfValue);
			damage = damage * (1 - epfValue / 25);


			Log.Debug($"Original Damage={originalDamage:F1} Redused Damage={damage:F1}, Armor Value={armorValue:F1}, EPF {epfValue:F1}");
			return (int) damage;

			//armorValue *= 0.04; // Each armor point represent 4% reduction
			//return (int) Math.Floor(damage*(1.0 - armorValue));
		}

		protected virtual double CalculateDamageReductionFromEnchantments(Entity source, Item armor, Item tool, DamageCause cause)
		{
			if (armor == null) return 0;
			if (armor.ExtraData == null) return 0;

			double reduction = 0;
			{
				var enchantings = armor.GetEnchantings();
				foreach (var enchanting in enchantings)
				{
					double typeModifier = 0;

					EnchantingType enchantingType = enchanting.Id;
					switch (enchantingType)
					{
						case EnchantingType.Protection:
							typeModifier = 1;
							break;
						case EnchantingType.FireProtection:
							if (cause == DamageCause.FireTick)
							{
								typeModifier = 2;
							}
							break;
						case EnchantingType.BlastProtection:
							// Not handled right now
							//typeModifier = 2;
							break;
						case EnchantingType.ProjectileProtection:
							if (source is Arrow)
							{
								typeModifier = 2;
							}
							break;
						case EnchantingType.FeatherFalling:
							if (cause == DamageCause.Fall)
							{
								typeModifier = 3;
							}
							break;
						case EnchantingType.Thorns:
							// Refactor: Make damage to the attacker (!)
							break;
						case EnchantingType.Respiration:
							// HealthManager air-ticks
							break;
						case EnchantingType.DepthStrider:
							break;
						case EnchantingType.AquaAffinity:
							break;
						case EnchantingType.Sharpness:
							break;
						case EnchantingType.Smite:
							break;
						case EnchantingType.BaneOfArthropods:
							break;
						case EnchantingType.Knockback:
							// Need to deal with for all knockbacks
							break;
						case EnchantingType.FireAspect:
							// Set target on fire. Need to deal with in "take hit" perhaps?
							break;
						case EnchantingType.Looting:
							break;
						case EnchantingType.Efficiency:
							break;
						case EnchantingType.SilkTouch:
							break;
						case EnchantingType.Unbreaking:
							break;
						case EnchantingType.Fortune:
							break;
						case EnchantingType.Power:
							break;
						case EnchantingType.Punch:
							break;
						case EnchantingType.Flame:
							break;
						case EnchantingType.Infinity:
							break;
						case EnchantingType.LuckOfTheSea:
							break;
						case EnchantingType.Lure:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					reduction += enchanting.Level * typeModifier;
				}
			}

			return reduction;
		}

		public int CalculateFireTickReduction(Player target)
		{
			int reduction = 0;
			reduction = Math.Max(reduction, target.Inventory.Helmet.GetEnchantingLevel(EnchantingType.FireProtection) * 15);
			reduction = Math.Max(reduction, target.Inventory.Chest.GetEnchantingLevel(EnchantingType.FireProtection) * 15);
			reduction = Math.Max(reduction, target.Inventory.Leggings.GetEnchantingLevel(EnchantingType.FireProtection) * 15);
			reduction = Math.Max(reduction, target.Inventory.Boots.GetEnchantingLevel(EnchantingType.FireProtection) * 15);

			return (int) Math.Ceiling(reduction / 100f);
		}

		public int CalculateKnockback(Item tool)
		{
			return tool.GetEnchantingLevel(EnchantingType.Knockback);
		}
	}
}