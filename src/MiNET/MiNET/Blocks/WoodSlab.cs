using System.Numerics;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class WoodSlab : Block
	{
		public WoodSlab() : base(158)
		{
			BlastResistance = 15;
			Hardness = 2;
			IsFlammable = true;
		}

	    public override BoundingBox GetBoundingBox()
	    {
            return new BoundingBox(Coordinates, (Vector3) Coordinates + new Vector3(1f, 0.5f, 1f));
	    }
	}
}