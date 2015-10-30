using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Stone : Block
	{
		public Stone() : base(1)
		{
			IsReplaceable = false; //Will it be replaced, if a block is placed at its location?
			IsSolid = true; //Will it stop you if you try going through it?
			IsBuildable = true; //Can it be placed?
			IsTransparent = false; //Can light go through it?
			MetaData = 0; //Item meta data | 0-15
			Hardness = 1.5; //Block breaking resistance amount
			BlastResistance = 30; //Blast resistance amount
			
			/**
			IsBreakable = true; //Can it be broken?
			Emit = false; //Does it emit light?
			LightLevel = null; //Emite Amount | null or 0-15
			HasMeta = false; //Are there alternative versions?
			HasDrops = true; //Does it drop anything when broke?
			Stackable = true; //Will it stack with the same item?
			StackAmount = 64; //How many items can be in the itemstack | 1-64
			**/
		}
		
		/**
		//Check if tool has silk and if the tool is strong enough to drop anything
		public override ItemStack GetDrops(Boolean silk, Float toolHardness)
		{
			if (toolHardness < Hardness)
			{
				return null; //Drop nothing - the tool isn't strong enough
			}
			else
			{
				if (silk == true)
				{
					return new ItemStack(1); // Drop stone - the tool has silk
				}
				else
				{
					return new ItemStack(4); // Drop cobblestone - the default
				}
			}
		}
		**/
		
		
		//Replace this with the something similar to above
		public override ItemStack GetDrops()
		{
			return new ItemStack(4); // Drop cobblestone - the default
		}
	}
}
