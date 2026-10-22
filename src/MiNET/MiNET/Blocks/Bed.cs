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
using MiNET.Entities.Hostile;
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

		public Bed()
		{
			//IsFlammable = true; // It can catch fire from lava, but not other means.
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItemByName("minecraft:bed", Color)};
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

			BlockCoordinates head = GetHeadCoordinates();
			foreach (Player sleeper in level.GetSpawnedPlayers())
			{
				if (sleeper.SleepingPosition == head) sleeper.StopSleep();
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

		private const float ReachDistance = 2;
		private const int MonsterHorizontalRange = 8;
		private const int MonsterVerticalRange = 5;

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			Bed head = HeadPieceBit ? this : world.GetBlock(GetOtherPart()) as Bed;
			if (head == null || !head.HeadPieceBit) return true;

			if (world.Dimension != Dimension.Overworld)
			{
				if (world.RespawnBlocksExplode) new Explosion(world, head.Coordinates, 5, true).Explode();
				return true;
			}

			if (!IsInReach(player.KnownPosition, Coordinates) && !IsInReach(player.KnownPosition, GetOtherPart()))
			{
				player.SendMessage("tile.bed.tooFar", MessageType.Translation, needsTranslation: true);
				return true;
			}

			if (head.OccupiedBit)
			{
				player.SendMessage("tile.bed.occupied", MessageType.Translation, needsTranslation: true);
				return true;
			}

			player.SetRespawnBed(head.Coordinates);

			if (!IsSleepTime(world))
			{
				player.SendMessage("tile.bed.noSleep", MessageType.Translation, needsTranslation: true);
				return true;
			}

			if (AreMonstersNear(world, head.Coordinates))
			{
				player.SendMessage("tile.bed.notSafe", MessageType.Translation, needsTranslation: true);
				return true;
			}

			player.SleepOn(head);
			return true;
		}

		private static bool IsInReach(PlayerLocation position, BlockCoordinates half)
		{
			return Math.Abs(position.X - (half.X + 0.5f)) <= ReachDistance
					&& Math.Abs(position.Y - half.Y) <= ReachDistance
					&& Math.Abs(position.Z - (half.Z + 0.5f)) <= ReachDistance;
		}

		private static bool IsSleepTime(Level world)
		{
			long time = world.CurrentWorldCycleTime;
			return world.RainLevel > 0
				? time >= 12002 && time <= 23998
				: time >= 12523 && time <= 23477;
		}

		private static bool AreMonstersNear(Level world, BlockCoordinates head)
		{
			foreach (Entity entity in world.Entities.Values)
			{
				if (entity is not HostileMob || entity.HealthManager.IsDead) continue;

				PlayerLocation position = entity.KnownPosition;
				if (Math.Abs(position.X - (head.X + 0.5f)) <= MonsterHorizontalRange
					&& Math.Abs(position.Z - (head.Z + 0.5f)) <= MonsterHorizontalRange
					&& Math.Abs(position.Y - head.Y) <= MonsterVerticalRange)
				{
					return true;
				}
			}

			return false;
		}

		public BlockCoordinates GetHeadCoordinates()
		{
			return HeadPieceBit ? Coordinates : GetOtherPart();
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