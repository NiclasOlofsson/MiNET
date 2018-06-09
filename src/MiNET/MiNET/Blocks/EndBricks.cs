namespace MiNET.Blocks
{
	public class EndBricks : Block
	{
		public EndBricks() : base(206)
		{
			BlastResistance = 4;
			Hardness = 0.8f;
            IsConductive = true;
        }
	}
}
