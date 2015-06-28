namespace MiNET.Blocks
{
	internal class Farmland : Block
	{
		internal Farmland() : base(60)
		{
		}

     	public override ItemStack GetDrops()
		{
		return new ItemStack(3, 1); // Drop dirt block
		}
	}
}