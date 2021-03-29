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

using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class DoublePlant : Block
	{
		public DoublePlant() : base(175)
		{
			BlastResistance = 3;
			Hardness = 0.6f;

			IsSolid = false;
			IsReplaceable = true;
			IsTransparent = true;
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			if (base.CanPlace(world, player, blockCoordinates, targetCoordinates, face))
			{
				Block under = world.GetBlock(Coordinates.BlockDown());
				return under is Grass || under is Dirt;
			}

			return false;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates.BlockDown() == blockCoordinates)
			{
				level.SetAir(Coordinates);
				UpdateBlocks(level);
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			if (UpperBlockBit) return base.GetDrops(tool);

			return new Item[0];
		}
	}
}