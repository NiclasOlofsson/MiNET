using System;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class TallGrass : Block
	{
		public enum TallGrassTypes
		{
			DeadShrub = 0,
			TallGrass = 1,
			Fern = 2
		}

		public TallGrass() : base(31)
		{
			BlastResistance = 3;
			Hardness = 0.6f;

			IsSolid = false;
			IsReplacible = true;
			IsTransparent = true;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			base.OnTick(level, isRandom);

			if (isRandom)
			{

			}
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates + BlockCoordinates.Down == blockCoordinates)
			{
				level.SetAir(Coordinates);
				UpdateBlocks(level);
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			// 50% chance to drop seeds.
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if (rnd.NextDouble() > 0.5)
			{
				return new[] {ItemFactory.GetItem(295)};
			}

			return new Item[0];
		}
	}
}