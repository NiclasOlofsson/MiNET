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

using System;
using System.Numerics;
using log4net;
using MiNET.BlockEntities;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Bed : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Bed));

		public byte Color { get; set; }

		//[StateRange(0, 3)] public int Direction { get; set; } = 0;
		//[StateBit] public bool HeadPieceBit { get; set; } = true;
		//[StateBit] public bool OccupiedBit { get; set; } = false;

		public Bed() : base(26)
		{
			BlastResistance = 1;
			Hardness = 0.2f;
			IsTransparent = true;
			//IsFlammable = true; // It can catch fire from lava, but not other means.
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(355, Color)};
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			Direction = player.GetDirectionEmum() switch
			{
				Entity.Direction.West => 0,
				Entity.Direction.North => 1,
				Entity.Direction.East => 2,
				Entity.Direction.South => 3,
				_ => throw new ArgumentOutOfRangeException()
			};

			return world.GetBlock(blockCoordinates).IsReplaceable && world.GetBlock(GetOtherPart()).IsReplaceable;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			HeadPieceBit = false;
			world.SetBlock(new GoldBlock() {Coordinates = Coordinates + BlockCoordinates.Down});
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = Coordinates,
				Color = Color
			});

			var otherCoord = GetOtherPart();
			Bed blockOther = new Bed
			{
				Coordinates = otherCoord,
				Direction = Direction,
				HeadPieceBit = true,
			};
			world.SetBlock(blockOther);
			world.SetBlock(new GoldBlock() {Coordinates = otherCoord + BlockCoordinates.Down});
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = blockOther.Coordinates,
				Color = Color
			});

			return false;
		}

		public override void BreakBlock(Level level, BlockFace face, bool silent = false)
		{
			if (level.GetBlockEntity(Coordinates) is BedBlockEntity blockEntiy)
			{
				Color = blockEntiy.Color;
			}

			base.BreakBlock(level, face, silent);

			var other = GetOtherPart();
			level.SetAir(other);
			level.RemoveBlockEntity(other);
		}

		private BlockCoordinates GetOtherPart()
		{
			var direction = Direction switch
			{
				0 => Level.North,
				1 => Level.East,
				2 => Level.South,
				3 => Level.West,
				_ => throw new ArgumentOutOfRangeException()
			};

			if (!HeadPieceBit)
			{
				direction = direction * -1;
			}

			return Coordinates + direction;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			if (OccupiedBit)
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
			Bed other = world.GetBlock(GetOtherPart()) as Bed;
			if (other == null) return;

			OccupiedBit = isOccupied;
			other.OccupiedBit = isOccupied;
			world.SetBlock(this);
			world.SetBlock(other);

			//if (isOccupied)
			//{
			//	OccupiedBit = false;
			//	world.SetData(Coordinates, (byte) (Metadata | 0x04));
			//	world.SetData(other.Coordinates, (byte) (other.Metadata | 0x04));
			//}
			//else
			//{
			//	world.SetData(Coordinates, (byte) (Metadata & ~0x04));
			//	world.SetData(other.Coordinates, (byte) (other.Metadata & ~0x04));
			//}
		}
	}
}