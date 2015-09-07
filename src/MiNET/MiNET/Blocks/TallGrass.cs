namespace MiNET.Blocks
{
	public class TallGrass : Block
	{
		public enum TallGrassTypes
		{
			DeadShrub = 0,
			TallGrass = 1,
			Fern = 2
		}

		public TallGrass() : base(31)
		{
			IsSolid = false;
			IsReplacible = true;
			IsTransparent = true;
		}
	}
}