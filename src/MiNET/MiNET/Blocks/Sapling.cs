using MiNET.Utils;
using MiNET.Worlds;
using MiNET.Worlds.Structures;

namespace MiNET.Blocks
{
	public class Sapling : Block
	{
		public Sapling() : base(6)
		{
			FuelEfficiency = 5;
			IsTransparent = true;
			IsFlammable = true;
		}

		public SaplingType GetSaplingType()
		{
			byte result = 0;
			BitHelper.SetBit(ref result, 0x00, BitHelper.GetBit(Metadata, 0x00));
			BitHelper.SetBit(ref result, 0x01, BitHelper.GetBit(Metadata, 0x01));
			BitHelper.SetBit(ref result, 0x02, BitHelper.GetBit(Metadata, 0x02));

			return (SaplingType) result;
		}

		public override void OnTick(Level level, bool isRandom)
		{
		 //	level.BroadcastMessage(
		//		$"IsRandom: {isRandom} | SkyLight: {SkyLight} | BlockLight: {BlockLight} | Type: {GetSaplingType().ToString()} | Coordinates: {Coordinates}");
			if (SkyLight < 9) return;

		//	level.BroadcastMessage("Sapling pass 1");

		//	if (isRandom)
		//	{
				const int pos = 0x07;
				bool readyToGrow = BitHelper.GetBit(Metadata, pos);

			if (isRandom)
			{
				//For some reason setting the "Ready To Grow" flag in the metadata doesn't work,
				//Everytime i set it to true, next time we hit the random tick it says its false again.
				if (!readyToGrow)
				{
					/*var oldMeta = Metadata;
					var newMetadata = Metadata;
					BitHelper.SetBit(ref newMetadata, pos, true);
					Metadata = newMetadata;*/

					level.ScheduleBlockTick(this, new FastRandom().Next(20, 200));

					//level.SetBlock(this);

					//	level.SetData(Coordinates, newMetadata);

					/*level.BroadcastMessage(
						$"Old: {oldMeta}, {pos}= {BitHelper.GetBit(oldMeta, pos)}\nNew: {newMetadata}, {pos}= {BitHelper.GetBit(newMetadata, pos)}");*/
				}
			}
			else
			{
				//else
				{
					bool was2by2 = false;
					TreeStructure tree = null;
					switch (GetSaplingType())
					{
						case SaplingType.Oak: //Oak
							tree = new OakTree();
							break;
						case SaplingType.Spruce: //Spruce
							tree = new SpruceTree();
							break;
						case SaplingType.Birch: //Birch
							tree = new BirchTree();
							break;
						case SaplingType.Jungle: //Jungle
							if (Check2By2(level))
							{
								tree = new LargeJungleTree();
								was2by2 = true;
							}
							else
							{
								tree = new SmallJungleTree();
							}
							break;
						case SaplingType.Acacia: //Acacia
							tree = new AcaciaTree();
							break;
						case SaplingType.DarkOak: //Dark Oak
							tree = new OakTree(); //TODO: Add dark oak tree's
							break;
					}

					if (tree != null)
					{
						tree.Create(level, Coordinates.X, Coordinates.Y, Coordinates.Z);
						if (was2by2)
						{
							CheckAndRemove(level, new BlockCoordinates(0, 0, 0));
							CheckAndRemove(level, new BlockCoordinates(1, 0, 0));
							CheckAndRemove(level, new BlockCoordinates(0, 0, 1));
							CheckAndRemove(level, new BlockCoordinates(1, 0, 1));

							CheckAndRemove(level, new BlockCoordinates(-1, 0, 0));
							CheckAndRemove(level, new BlockCoordinates(0, 0, -1));
							CheckAndRemove(level, new BlockCoordinates(-1, 0, -1));
						}
					}
				}
			}
			//}
		}

		private bool CheckAndRemove(Level level, BlockCoordinates offset)
		{
			if (level.GetBlock(Coordinates + offset).Id == Id)
			{
				level.SetAir(Coordinates + offset);
				return true;
			}
			return false;
		}

		private bool Check2By2(Level level)
		{
			if ((ValidBlock(
				    level.GetBlock(Coordinates + new BlockCoordinates(1, 0, 0))) || ValidBlock(
					level.GetBlock(Coordinates + new BlockCoordinates(-1, 0, 0))))
				&&
			    (ValidBlock(
				    level.GetBlock(Coordinates + new BlockCoordinates(0, 0, 1))) || ValidBlock(
					level.GetBlock(Coordinates + new BlockCoordinates(0, 0, -1))))
			    &&
			    (ValidBlock(
				    level.GetBlock(Coordinates + new BlockCoordinates(1, 0, 1))) || ValidBlock(
					level.GetBlock(Coordinates + new BlockCoordinates(-1, 0, -1)))))
				return true;

			return false;
		}

		private bool ValidBlock(Block block)
		{
			Sapling sapling = block as Sapling;
			if (sapling == null) return false;

			if (sapling.GetSaplingType() == GetSaplingType())
			{
				return true;
			}

			return false;
		}

		public enum SaplingType : byte
		{
			Oak = 0,
			Spruce = 1,
			Birch = 2,
			Jungle = 3,
			Acacia = 4,
			DarkOak = 5
		}
	}
}
