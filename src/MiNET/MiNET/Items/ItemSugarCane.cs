using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public partial class ItemSugarCane : ItemBlock
	{
		public override Block Block { get; protected set; } = new Reeds();
	}
}
