﻿#region LICENSE

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
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class IronTrapdoor : Block
	{
		public IronTrapdoor() : base()
		{
			IsTransparent = true;
			BlastResistance = 25;
			Hardness = 5;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			UpsideDownBit = faceCoords.Y > 0.5 && face != BlockFace.Up || face == BlockFace.Down;

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			OpenBit = !OpenBit;
			world.SetBlock(this);

			return true;
		}
	}
}