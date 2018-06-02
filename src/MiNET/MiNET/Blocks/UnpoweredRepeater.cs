namespace MiNET.Blocks
{
	public class UnpoweredRepeater : Block
	{
		public UnpoweredRepeater() : this(93)
        {
            IsConductive = true;
        }

		public UnpoweredRepeater(byte id) : base(id)
		{
			IsTransparent = true;
            IsConductive = true;
        }
	}
}