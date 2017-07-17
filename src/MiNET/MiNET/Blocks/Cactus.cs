using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Cactus : Block
	{
		public Cactus() : base(81)
		{
			IsTransparent = true;
			BlastResistance = 2;
			Hardness = 0.4f;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			var up = Coordinates + BlockCoordinates.Up;

			Block twoBlockDowns = level.GetBlock(Coordinates + (BlockCoordinates.Down + BlockCoordinates.Down));
			if (twoBlockDowns.Id != Id && level.GetBlock(up).Id != Id) //If 2 blocks beneath us && the block above us is not sugar cane, then do growing.
			{
				if (Metadata >= 3) //Ready to grow.
				{
					if (level.IsTransparent(up))
					{
						level.SetData(Coordinates, 0); //Reset growth level.

						Block newReeds = BlockFactory.GetBlockById(Id);
						newReeds.Metadata = 0;
						newReeds.Coordinates = up;

						level.SetBlock(newReeds); //Plant new sugar cane.
					}
				}
				else if (isRandom && twoBlockDowns.Id != Id && Metadata < 15) //Do not do the ageing when we are already 3 blocks tall or when we are already at growth 15
				{
					level.SetData(Coordinates, (byte)(Metadata + 1));
				}
			}

		//	level.BroadcastMessage($"Cactus at {Coordinates} ticked. | Meta: {Metadata} / 15 | IsRandom: {isRandom}");
		}
	}
}