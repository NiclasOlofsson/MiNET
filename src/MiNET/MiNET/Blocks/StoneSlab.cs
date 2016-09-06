using System.Numerics;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class StoneSlab : Block
	{
		public StoneSlab() : base(44)
		{
			BlastResistance = 30;
			Hardness = 2;
			IsTransparent = true; // Partial
        }

        public override BoundingBox GetBoundingBox()
        {
            return new BoundingBox(Coordinates, (Vector3)Coordinates + new Vector3(1f, 0.5f, 1f));
        }
    }
}