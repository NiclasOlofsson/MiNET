using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MiNET.Utils;

namespace MiNET.Worlds.Structures
{
	public class LargeJungleTree : Structure
	{
		public override string Name
		{
			get { return "JungleTree"; }
		}

		public override int MaxHeight
		{
			get { return 30; }
		}

		public override void Create(ChunkColumn chunk, int x, int y, int z)
		{
			var block = chunk.GetBlock(x, y - 1, z);
			if (block != 2 && block != 3) return;

			var location = new Vector3(x, y, z);

			int height = Math.Max(8, new Random().Next(30));

			GenerateColumn(chunk, location, height, 17, 3);
			Vector3 leafLocation = location + new Vector3(0, height, 0);
			GenerateVanillaLeaves(chunk, leafLocation, 2, 18, 3);
		}
	}
}
