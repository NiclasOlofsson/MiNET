using MiNET.Utils;

﻿namespace MiNET.Blocks
{
	public class Farmland : Block
	{
		public Farmland() : base(60)
		{
		}

     
		public override ItemStack GetDrops()
		{
			return new ItemStack(3); // Drop dirt block
		}
	}
}