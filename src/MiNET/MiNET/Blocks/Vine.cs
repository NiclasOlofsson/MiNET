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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Vine : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Vine));

		public Vine() : base(106)
		{
			IsSolid = false;
			IsTransparent = false;
			BlastResistance = 1;
			Hardness = 0.2f;
			IsFlammable = true;
			IsReplacible = true;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(Coordinates);

			if (block is Vine)
			{
				Metadata = block.Metadata;
			}

			Log.Debug($"Face: {face}, CurrentBlock={block}");

			byte direction;
			switch (face)
			{
				case BlockFace.North:
					direction = 0x01;
					break;
				case BlockFace.South:
					direction = 0x04;
					break;
				case BlockFace.West:
					direction = 0x08;
					break;
				case BlockFace.East:
					direction = 0x02;
					break;
				default:
					return true; // Do nothing
			}

			if ((Metadata & direction) == direction) return true; // Already have this face covered

			Metadata |= direction;

			world.SetBlock(this);
			return true;
		}
	}
}