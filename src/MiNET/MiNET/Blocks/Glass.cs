namespace MiNET.Blocks
{
	public class Glass : Block
	{
		public Glass() : this(20)
		{
			
		}
		public Glass(byte id) : base(id)
		{
			IsTransparent = true;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
			IsBlockingSkylight = false;
		}
	}
}