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

using System.Collections.Generic;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class EnchantingTable : Block
	{
		public EnchantingTable() : base(116)
		{
			FuelEfficiency = 15;
			IsTransparent = true;
			BlastResistance = 6000;
			Hardness = 5;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			//switch (direction)
			//{
			//	case 1:
			//		Metadata = 2;
			//		break; // West
			//	case 2:
			//		Metadata = 5;
			//		break; // North
			//	case 3:
			//		Metadata = 3;
			//		break; // East
			//	case 0:
			//		Metadata = 4;
			//		break; // South 
			//}

			var tableBlockEntity = new EnchantingTableBlockEntity {Coordinates = Coordinates};

			world.SetBlockEntity(tableBlockEntity);

			return false;
		}


		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			
			player.OpenInventory(blockCoordinates);

			return true;
		}
	}
}