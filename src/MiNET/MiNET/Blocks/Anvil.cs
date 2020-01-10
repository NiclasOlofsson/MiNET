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
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Anvil : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Anvil));

		public const int Normal = 0;
		public const int SlightlyDamaged = 4;
		public const int VeryDamaged = 8;

		public Anvil() : base(145)
		{
			IsTransparent = true;
			BlastResistance = 6000;
			Hardness = 5;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();
			Metadata = (byte) ((Metadata & 0x0c) | (direction & 0x03));

			return false;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var containerOpen = McpeContainerOpen.CreateObject();
			containerOpen.windowId = 5 + 9;
			containerOpen.type = 5;
			containerOpen.coordinates = blockCoordinates;
			containerOpen.runtimeEntityId = -1;
			player.SendPacket(containerOpen);

			return true;
		}
	}
}