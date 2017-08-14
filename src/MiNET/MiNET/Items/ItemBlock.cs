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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	/// <summary>
	///     Generic Item that will simply place the block on use. No interaction or other use supported by the block.
	/// </summary>
	public class ItemBlock : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemBlock));

		protected Block _block;

		public Block Block => _block;

		protected ItemBlock(short id, short metadata) : base(id, metadata)
		{
		}

		public ItemBlock(Block block, short metadata) : base(block.Id, metadata)
		{
			_block = block;
			FuelEfficiency = _block.FuelEfficiency;
		}

		public override Item GetSmelt()
		{
			return _block.GetSmelt();
		}


		public override void UseItem(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(targetCoordinates);
			_block.Coordinates = block.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			_block.Metadata = (byte) Metadata;

			if ((player.GetBoundingBox() - 0.01f).Intersects(_block.GetBoundingBox()))
			{
				Log.Debug("Can't build where you are standing: " + _block.GetBoundingBox());
				return;
			}
			if (!_block.CanPlace(world, targetCoordinates, face)) return;

			if (_block.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(_block);
		}
	}
}