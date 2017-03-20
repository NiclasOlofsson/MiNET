using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Grass : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Grass));

		public Grass() : base(2)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (!isRandom) return;

			if (!level.IsTransparent(Coordinates + BlockCoordinates.Up))
			{
				Block dirt = BlockFactory.GetBlockById(3);
				dirt.Coordinates = Coordinates;
				level.SetBlock(dirt, true, false, false);
			}

			//TODO: Do grass spreading here
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; //Drop dirt block
		}
	}
}