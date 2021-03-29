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

		protected BlockStairs(int id) : base(id)
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


	public partial class PrismarineStairs : BlockStairs { public PrismarineStairs() : base(257) { IsGenerated = false; } }
	public partial class DarkPrismarineStairs : BlockStairs { public DarkPrismarineStairs() : base(258) { IsGenerated = false; } }
	public partial class PrismarineBricksStairs : BlockStairs { public PrismarineBricksStairs() : base(259) { IsGenerated = false; } }
	public partial class GraniteStairs : BlockStairs { public GraniteStairs() : base(424) { IsGenerated = false; } }
	public partial class DioriteStairs : BlockStairs { public DioriteStairs() : base(425) { IsGenerated = false; } }
	public partial class AndesiteStairs : BlockStairs { public AndesiteStairs() : base(426) { IsGenerated = false; } }
	public partial class PolishedGraniteStairs : BlockStairs { public PolishedGraniteStairs() : base(427) { IsGenerated = false; } }
	public partial class PolishedDioriteStairs : BlockStairs { public PolishedDioriteStairs() : base(428) { IsGenerated = false; } }
	public partial class PolishedAndesiteStairs : BlockStairs { public PolishedAndesiteStairs() : base(429) { IsGenerated = false; } }
	public partial class MossyStoneBrickStairs : BlockStairs { public MossyStoneBrickStairs() : base(430) { IsGenerated = false; } }
	public partial class SmoothRedSandstoneStairs : BlockStairs { public SmoothRedSandstoneStairs() : base(431) { IsGenerated = false; } }
	public partial class SmoothSandstoneStairs : BlockStairs { public SmoothSandstoneStairs() : base(432) { IsGenerated = false; } }
	public partial class EndBrickStairs : BlockStairs { public EndBrickStairs() : base(433) { IsGenerated = false; } }
	public partial class MossyCobblestoneStairs : BlockStairs { public MossyCobblestoneStairs() : base(434) { IsGenerated = false; } }
	public partial class NormalStoneStairs : BlockStairs { public NormalStoneStairs() : base(435) { IsGenerated = false; } }
	public partial class RedNetherBrickStairs : BlockStairs { public RedNetherBrickStairs() : base(439) { IsGenerated = false; } }
	public partial class SmoothQuartzStairs : BlockStairs { public SmoothQuartzStairs() : base(440) { IsGenerated = false; } }
	public partial class CrimsonStairs : BlockStairs { public CrimsonStairs() : base(509) { IsGenerated = false; } }
	public partial class WarpedStairs : BlockStairs { public WarpedStairs() : base(510) { IsGenerated = false; } }
	public partial class PolishedBlackstoneBrickStairs : BlockStairs { public PolishedBlackstoneBrickStairs() : base(530) { IsGenerated = false; } }
	public partial class BlackstoneStairs : BlockStairs { public BlackstoneStairs() : base(531) { IsGenerated = false; } }
	public partial class PolishedBlackstoneStairs : BlockStairs { public PolishedBlackstoneStairs() : base(547) { IsGenerated = false; } }
}