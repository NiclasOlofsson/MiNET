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
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class GlazedTerracotta : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (GlazedTerracotta));

		private byte _baseDirection = 0;
		private static long _nextTick = 0;
		private static long _tickOn = 0;
		private static long _tick = 0;

		public GlazedTerracotta(byte id) : base(id)
		{
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (isRandom) return;

			if (_nextTick == 0 || _nextTick <= level.TickTime)
			{
				_tick++;
				_nextTick = level.TickTime + 5;
				_tickOn = level.TickTime;
			}

			if (level.TickTime == _tickOn)
			{
				var blockEntity = (BedBlockEntity) level.GetBlockEntity(Coordinates);
				if (blockEntity != null)
				{
					_baseDirection = blockEntity.Color;

					var d = _tick;
					byte direction = (byte) (d + _baseDirection);

					direction = (byte) ((direction)%(3 + 1));

					//Log.Warn($"Tick ({d})! Direction {direction}, base {_baseDirection}");

					switch (direction)
					{
						case 1:
							Metadata = 2;
							break; // West
						case 2:
							Metadata = 5;
							break; // North
						case 3:
							Metadata = 3;
							break; // East
						case 0:
							Metadata = 4;
							break; // South 
					}

					level.SetBlock(this, true, false, false);
				}
			}

			level.ScheduleBlockTick(this, 1);
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();
			_baseDirection = direction;

			switch (direction)
			{
				case 1:
					Metadata = 2;
					break; // West
				case 2:
					Metadata = 5;
					break; // North
				case 3:
					Metadata = 3;
					break; // East
				case 0:
					Metadata = 4;
					break; // South 
			}

			world.SetBlock(this);
			world.SetBlockEntity(new BedBlockEntity
			{
				Coordinates = this.Coordinates,
				Color = (byte) direction
			}, false);

			world.ScheduleBlockTick(this, 10);

			return true;
		}
	}
}