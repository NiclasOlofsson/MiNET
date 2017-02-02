namespace MiNET.Blocks
{
	public class Glass : Block
	{
		internal Glass() : this(20)
		{
			
		}
		public Glass(int id) : base(id)
		{
			IsTransparent = true;
			BlastResistance = 1.5f;
			Hardness = 0.3f;
		}
	}
}