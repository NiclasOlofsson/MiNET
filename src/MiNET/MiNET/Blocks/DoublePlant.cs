using MiNET.Items;

namespace MiNET.Blocks
{
	public class DoublePlant : Block
	{
		public DoublePlant() : base(175)
		{
			BlastResistance = 3;
			Hardness = 0.6f;

			IsSolid = false;
			IsReplacible = true;
			IsTransparent = true;
		}

		public override Item[] GetDrops()
		{
			//return new Item[] { new ItemBlock(this, (short)(Metadata)) { Count = 1 } };

			if ((Metadata & 0x08) == 0x08)
			{
				return new Item[] { new ItemBlock(this, (short)(Metadata & 0x07)) { Count = 1 } };
			}

			return new Item[0];
		}
	}
}