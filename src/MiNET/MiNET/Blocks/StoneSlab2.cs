

using System.Numerics;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class StoneSlab2 : Block
	{
		public StoneSlab2() : base(182)
		{

		}

		public override BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Coordinates, (Vector3)Coordinates + new Vector3(1f, 0.5f, 1f));
		}
	}
}