using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class Cocoa : Block
	{
		public Cocoa() : base(127)
		{
			IsTransparent = true;
			BlastResistance = 15;
			Hardness = 0.2f;
		}
	}
}