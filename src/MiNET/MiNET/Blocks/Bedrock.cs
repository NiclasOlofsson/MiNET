using MiNET.Items;

namespace MiNET.Blocks
{
	public class Bedrock : Block
	{
		public Bedrock() : base(7)
		{
			Hardness = 60000;
			BlastResistance = 18000000;
		}


		public override Item[] GetDrops(Item tool)
		{
			return new Item[0]; //Drop nothing
		}
	}
}