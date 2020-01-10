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
using MiNET.BlockEntities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Bed : Block
	{
		public Bed() : base(26)
		{
			BlastResistance = 1;
			Hardness = 0.2f;
			IsTransparent = true;
			//IsFlammable = true; // It can catch fire from lava, but not other means.
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(355, _color)};
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			_color = Metadata; // from item

			byte direction = player.GetDirection();

			switch (direction)
			{
				case 1:
					Metadata = 0;
					break; // West
				case 2:
					Metadata = 1;
					break; // North
				case 3:
					Metadata = 2;
					break; // East
				case 0:
					Metadata = 3;
					break; // South 
			}

			return world.GetBlock(blockCoordinates).IsReplacible && world.GetBlock(GetOtherPart()).IsReplacible;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = Coordinates,
				Color = _color
			});

			var otherCoord = GetOtherPart();

			Bed blockOther = new Bed
			{
				Coordinates = otherCoord,
				Metadata = (byte) (Metadata | 0x08)
			};

			world.SetBlock(blockOther);
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = blockOther.Coordinates,
				Color = _color
			});

			return false;
		}

		public override void BreakBlock(Level level, bool silent = false)
		{
			if (level.GetBlockEntity(Coordinates) is BedBlockEntity blockEntiy)
			{
				_color = blockEntiy.Color;
			}

			base.BreakBlock(level, silent);

			var other = GetOtherPart();
			level.SetAir(other);
			level.RemoveBlockEntity(other);
		}

		private BlockCoordinates GetOtherPart()
		{
			BlockCoordinates direction = new BlockCoordinates();
			switch (Metadata & 0x07)
			{
				case 0:
					direction = Level.North;
					break; // West
				case 1:
					direction = Level.East;
					break; // South
				case 2:
					direction = Level.South;
					break; // East
				case 3:
					direction = Level.West;
					break; // North 
			}

			// Remove bed
			if ((Metadata & 0x08) != 0x08)
			{
				direction = direction * -1;
			}

			return Coordinates + direction;
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(Bed));
		private byte _color;

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			if ((Metadata & 0x04) == 0x04)
			{
				Log.Debug($"Bed at {Coordinates} is already occupied"); // Send proper message to player
				return true;
			}

			SetOccupied(world, true);
			player.IsSleeping = true;
			player.SpawnPosition = blockCoordinates;
			player.BroadcastSetEntityData();
			return true;
		}

		public void SetOccupied(Level world, bool isOccupied)
		{
			var other = world.GetBlock(GetOtherPart());
			if (isOccupied)
			{
				world.SetData(Coordinates, (byte) (Metadata | 0x04));
				world.SetData(other.Coordinates, (byte) (other.Metadata | 0x04));
			}
			else
			{
				world.SetData(Coordinates, (byte) (Metadata & ~0x04));
				world.SetData(other.Coordinates, (byte) (other.Metadata & ~0x04));
			}
		}
	}
}