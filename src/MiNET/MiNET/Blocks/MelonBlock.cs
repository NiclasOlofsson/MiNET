using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class MelonBlock : Block
	{
		public MelonBlock() : base(103)
		{
			Hardness = 1;
			IsTransparent = true;
            IsConductive = true;
        }

		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			return new[] {ItemFactory.GetItem(360, 0, (byte) (3 + rnd.Next(5)))};
		}
	}
}