

namespace MiNET.Blocks
{
	public class UnpoweredComparator : Block
	{
		public UnpoweredComparator() : this(149)
		{
			
		}

		public UnpoweredComparator(byte id) : base(id)
		{
			IsTransparent = true;
		}
	}
}