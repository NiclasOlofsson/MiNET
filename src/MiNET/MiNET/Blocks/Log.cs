using MiNET.Items;

namespace MiNET.Blocks
{
	public class Log : Block
	{
		public Log() : base(17)
		{
			FuelEfficiency = 15;
			BlastResistance = 10;
			Hardness = 2;
			IsFlammable = true;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(Id, (short) (Metadata & 0x03), 1)};
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(263, 1);
		}
	}
}