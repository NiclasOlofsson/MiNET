using System;
using fNbt;
using log4net;
using MiNET.Effects;
using MiNET.Items;
using MiNET.Net;

namespace MiNET
{
	public class DamageCalculator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (DamageCalculator));

		public double CalculateItemDamage(Player player, Item item, Player target)
		{
			return item.GetDamage(); //Item Damage.
		}

		public virtual double CalculateFallDamage(Player player, double damage, Player target)
		{
			var fallDamage = player.Level.Random.Next((int) (damage/2 + 2));

			McpeAnimate animate = McpeAnimate.CreateObject();
			animate.entityId = target.EntityId;
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
				effectDamage -= (effect.Level + 1)*4;
			}
			else if (player.Effects.TryGetValue(EffectType.Strength, out effect))
			{
				effectDamage += (effect.Level + 1)*3;
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
					increase += 1 + ((level - 1)*0.5);
				}
			}

			return increase;
		}

		public virtual double CalculatePlayerDamage(Player target, double damage)
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
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
					epfValue += CalculateDamageReducationFromEnchantments(armorPiece);
				}
			}

			damage = damage*(1 - Math.Max(armorValue/5, armorValue - damage/2)/25);

			epfValue = Math.Min(20, epfValue);
			damage = damage*(1 - epfValue/25);


			Log.Debug($"Original Damage={originalDamage:F1} Redused Damage={damage:F1}, Armor Value={armorValue:F1}, EPF {epfValue:F1}");
			return (int) damage;

			//armorValue *= 0.04; // Each armor point represent 4% reduction
			//return (int) Math.Floor(damage*(1.0 - armorValue));
		}

		protected virtual double CalculateDamageReducationFromEnchantments(Item armor)
		{
			if (armor == null) return 0;
			if (armor.ExtraData == null) return 0;

			NbtList enchantings;
			if (!armor.ExtraData.TryGet("ench", out enchantings)) return 0;

			double reduction = 0;
			foreach (NbtCompound enchanting in enchantings)
			{
				short level = enchanting["lvl"].ShortValue;

				double typeModifier = 0;
				short id = enchanting["id"].ShortValue;
				if (id == 0) typeModifier = 1;
				else if (id == 1) typeModifier = 2;
				else if (id == 2) typeModifier = 2;
				else if (id == 3) typeModifier = 2;
				else if (id == 4) typeModifier = 3;

				reduction += level*typeModifier;
			}

			return reduction;
		}
	}
}