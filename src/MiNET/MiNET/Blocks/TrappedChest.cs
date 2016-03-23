namespace MiNET.Blocks
{
	public class TrappedChest : Chest
	{
		public TrappedChest() : base(146)
		{
			IsTransparent = true;
			BlastResistance = 12.5f;
			Hardness = 2.5f;
		}
	}
}