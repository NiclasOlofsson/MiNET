using System;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Gravel : Block
	{
		public Gravel() : base(13)
		{
		}

	    public override ItemStack GetDrops()
	    {
	        Random random = new Random();
	        if (random.Next(10) == 0)
	        {
	            return new ItemStack(318, 1);
	        }
            return new ItemStack(13, 1);
	    }
	}
}