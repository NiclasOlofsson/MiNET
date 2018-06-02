using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class UnlitRedstoneTorch : Torch
	{
		public UnlitRedstoneTorch() : this(75)
		{

        }

		protected UnlitRedstoneTorch(byte id) : base(id)
        {
            IsConductive = true;
        }

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new RedstoneTorch(), 0)};
		}
	}
}