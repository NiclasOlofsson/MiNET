#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class BlockStairs : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockStairs));

		[StateBit] public virtual bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public virtual int WeirdoDirection { get; set; } = 0;

		protected BlockStairs() : base()
		{
			FuelEfficiency = 15;
			IsTransparent = true; // Partial - blocks light.
			IsBlockingSkylight = false; // Partial - blocks light.
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			UpsideDownBit = ((faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down);

			WeirdoDirection = player.GetProperDirection();

			world.SetBlock(this);
			return true;
		}
	}


	public partial class PrismarineStairs : BlockStairs { public PrismarineStairs() : base() { } }
	public partial class DarkPrismarineStairs : BlockStairs { public DarkPrismarineStairs() : base() { } }
	public partial class PrismarineBricksStairs : BlockStairs { public PrismarineBricksStairs() : base() { } }
	public partial class GraniteStairs : BlockStairs { public GraniteStairs() : base() { } }
	public partial class DioriteStairs : BlockStairs { public DioriteStairs() : base() { } }
	public partial class AndesiteStairs : BlockStairs { public AndesiteStairs() : base() { } }
	public partial class PolishedGraniteStairs : BlockStairs { public PolishedGraniteStairs() : base() { } }
	public partial class PolishedDioriteStairs : BlockStairs { public PolishedDioriteStairs() : base() { } }
	public partial class PolishedAndesiteStairs : BlockStairs { public PolishedAndesiteStairs() : base() { } }
	public partial class MossyStoneBrickStairs : BlockStairs { public MossyStoneBrickStairs() : base() { } }
	public partial class SmoothRedSandstoneStairs : BlockStairs { public SmoothRedSandstoneStairs() : base() { } }
	public partial class SmoothSandstoneStairs : BlockStairs { public SmoothSandstoneStairs() : base() { } }
	public partial class EndBrickStairs : BlockStairs { public EndBrickStairs() : base() { } }
	public partial class MossyCobblestoneStairs : BlockStairs { public MossyCobblestoneStairs() : base() { } }
	public partial class NormalStoneStairs : BlockStairs { public NormalStoneStairs() : base() { } }
	public partial class RedNetherBrickStairs : BlockStairs { public RedNetherBrickStairs() : base() { } }
	public partial class SmoothQuartzStairs : BlockStairs { public SmoothQuartzStairs() : base() { } }
	public partial class CrimsonStairs : BlockStairs { public CrimsonStairs() : base() { } }
	public partial class WarpedStairs : BlockStairs { public WarpedStairs() : base() { } }
	public partial class PolishedBlackstoneBrickStairs : BlockStairs { public PolishedBlackstoneBrickStairs() : base() { } }
	public partial class BlackstoneStairs : BlockStairs { public BlackstoneStairs() : base() { } }
	public partial class PolishedBlackstoneStairs : BlockStairs { public PolishedBlackstoneStairs() : base() { } }
}