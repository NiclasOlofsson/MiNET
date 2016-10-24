

using System.Numerics;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class RedSandstoneSlab : Block
	{
		public RedSandstoneSlab() : base(182)
		{

		}

		public override BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Coordinates, (Vector3)Coordinates + new Vector3(1f, 0.5f, 1f));
		}
	}
}