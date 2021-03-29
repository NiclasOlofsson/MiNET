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
using MiNET.BlockEntities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Beacon : Block
	{
		public Beacon() : base(138)
		{
			LightLevel = 15;
			BlastResistance = 15;
			Hardness = 3;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			BeaconBlockEntity blockEntity = new BeaconBlockEntity()
			{
				Coordinates = Coordinates
			};

			world.SetBlockEntity(blockEntity);

			//BuildPyramidLevels(world, 4);

			return false;
		}

		private void BuildPyramidLevels(Level level, int levels)
		{
			for (int i = 1; i < levels + 1; i++)
			{
				for (int x = -i; x < i + 1; x++)
				{
					for (int z = -i; z < i + 1; z++)
					{
						level.SetBlock(new IronBlock() {Coordinates = Coordinates + new BlockCoordinates(x, -i, z)});
					}
				}
			}
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var containerOpen = McpeContainerOpen.CreateObject();
			containerOpen.windowId = 5 + 9;
			containerOpen.type = 13;
			containerOpen.coordinates = blockCoordinates;
			containerOpen.runtimeEntityId = -1;
			player.SendPacket(containerOpen);

			return true;
		}
	}
}