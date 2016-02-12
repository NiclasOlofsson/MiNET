using MiNET.Items;

namespace MiNET.Blocks
{
	public class Bedrock : Block
	{
		public Bedrock() : base(7)
		{
			Hardness = 60000;
		}


		public override Item GetDrops()
		{
			return null; //Drop nothing
		}
	}
}