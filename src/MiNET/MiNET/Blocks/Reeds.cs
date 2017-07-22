using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Reeds : Block
	{
		public Reeds() : base(83)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates + BlockCoordinates.Down == blockCoordinates)
			{
				level.BreakBlock(this, null);
			}
		}

		public override void OnTick(Level level, bool isRandom)
		{
			var up = Coordinates + BlockCoordinates.Up;

			Block twoBlockDowns = level.GetBlock(Coordinates + (BlockCoordinates.Down + BlockCoordinates.Down));
			if (twoBlockDowns.Id != Id && level.GetBlock(up).Id != Id) //If 2 blocks beneath us && the block above us is not a cactus, then do growing.
			{
				if (Metadata >= 15) //Ready to grow.
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

			//	level.BroadcastMessage($"Reeds at {Coordinates} ticked. | Meta: {Metadata} / 15 | IsRandom: {isRandom}");
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] { ItemFactory.GetItem(338, 0, 1) };
		}
	}
}