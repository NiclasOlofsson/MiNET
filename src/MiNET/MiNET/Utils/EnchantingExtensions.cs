using System.Collections.Generic;
using fNbt;
using MiNET.Items;

namespace MiNET.Utils
{
	public enum EnchantingType
	{
		Protection = 0,
		FireProtection = 1,
		FeatherFalling = 2,
		BlastProtection = 3,
		ProjectileProtection = 4,
		Thorns = 5,
		Respiration = 6,
		DepthStrider = 7,
		AquaAffinity = 8,
		Sharpness = 9,
		Smite = 10,
		BaneOfArthropods = 11,
		Knockback = 12,
		FireAspect = 13,
		Looting = 14,
		Efficiency = 15,
		SilkTouch = 16,
		Durability = 17,
		Fortune = 18,
		Power = 19,
		Punch = 20,
		Flame = 21,
		Infinity = 22,
		LuckOfTheSea = 23,
		Lure = 24
	}

	public class Enchanting
	{
		public EnchantingType Id { get; set; }
		public short Level { get; set; }
	}

	public static class EnchantingExtensions
	{
		public static List<Enchanting> GetEnchantings(this Item tool)
		{
			var enchantings = new List<Enchanting>();

			if (tool == null) return enchantings;
			if (tool.ExtraData == null) return enchantings;

			NbtList enchantingsNbt;
			if (!tool.ExtraData.TryGet("ench", out enchantingsNbt)) return enchantings;

			double increase = 0;
			foreach (NbtCompound enchantingNbt in enchantingsNbt)
			{
				short level = enchantingNbt["lvl"].ShortValue;

				if (level == 0) continue;

				Enchanting enchanting = new Enchanting();
				enchanting.Level = level;

				short id = enchantingNbt["id"].ShortValue;
				enchanting.Id = (EnchantingType) id;
				enchantings.Add(enchanting);
			}

			return enchantings;
		}

		public static short GetEnchantingLevel(this Item tool, EnchantingType enchantingId)
		{
			if (tool == null) return 0;
			if (tool.ExtraData == null) return 0;

			NbtList enchantingsNbt;
			if (!tool.ExtraData.TryGet("ench", out enchantingsNbt)) return 0;

			double increase = 0;
			foreach (NbtCompound enchantingNbt in enchantingsNbt)
			{
				short level = enchantingNbt["lvl"].ShortValue;

				if (level == 0) continue;

				short id = enchantingNbt["id"].ShortValue;

				if (id == (int) enchantingId) return level;
			}

			return 0;
		}

		public static void SetEnchantings(this Item tool, List<Enchanting> enchantings)
		{
			if (tool == null) return;
			if (tool.ExtraData == null)
			{
				if (tool.ExtraData == null)
				{
					tool.ExtraData = new NbtCompound("tag");
				}
			}

			tool.ExtraData.Remove("ench");

			var nbtList = new NbtList("ench");

			foreach (var enchanting in enchantings)
			{
				nbtList.Add(new NbtCompound {new NbtShort("id", (short) enchanting.Id), new NbtShort("lvl", enchanting.Level)});
			}

			tool.ExtraData.Add(nbtList);
		}
	}
}