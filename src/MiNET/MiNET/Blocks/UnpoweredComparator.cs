

namespace MiNET.Blocks
{
	public class UnpoweredComparator : Block
	{
		public UnpoweredComparator() : this(149)
		{
            IsConductive = true;
        }

		public UnpoweredComparator(byte id) : base(id)
		{
			IsTransparent = true;
            IsConductive = true;
        }
	}
}