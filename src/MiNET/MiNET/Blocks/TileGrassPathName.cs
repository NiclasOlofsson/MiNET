namespace MiNET.Blocks
{
	internal class TileGrassPathName : Block
	{
		internal TileGrassPathName() : base(198)
		{
		}

   	public override ItemStack GetDrops()
		{
		return new ItemStack(3, 1); // Drop dirt block
		}
	}

}