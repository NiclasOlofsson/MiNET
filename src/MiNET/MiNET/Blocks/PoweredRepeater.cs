namespace MiNET.Blocks
{
	public class PoweredRepeater : UnpoweredRepeater
	{
		public PoweredRepeater() : base(94)
		{
            IsConductive = true;
        }
	}
}