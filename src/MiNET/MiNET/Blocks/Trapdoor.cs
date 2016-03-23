namespace MiNET.Blocks
{
	public class Trapdoor : Block
	{
		public Trapdoor() : this(96)
		{
			
		}

		public Trapdoor(byte id) : base(id)
		{
			IsTransparent = true;
			BlastResistance = 15;
			Hardness = 5;
		}
	}
}