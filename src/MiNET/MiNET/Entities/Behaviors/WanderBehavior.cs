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
using MiNET.Blocks;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Entities.Behaviors
{
	public class WanderBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(WanderBehavior));

		private readonly Mob _entity;
		private readonly double _speedMultiplier;
		private readonly int _chance;
		private Path _currentPath;

		protected Vector3? _direction;

		public WanderBehavior(Mob entity, double speedMultiplier, int chance = 120)
		{
			_entity = entity;
			_speedMultiplier = speedMultiplier;
			_chance = chance;
		}

		public override bool ShouldStart()
		{
			if (_entity.Level.Random.Next(_chance) != 0) return false;

			BlockCoordinates? pos = FindRandomTargetBlock(_entity, 10, 7, _direction);

			if (!pos.HasValue) return false;

			var pathfinder = new Pathfinder();
			_currentPath = pathfinder.FindPath(_entity, pos.Value, pos.Value.DistanceTo((BlockCoordinates) _entity.KnownPosition) + 2);

			return _currentPath.HavePath();
		}

		private BlockCoordinates _lastPosition;
		private int _stallTime = 0;

		public override bool CanContinue()
		{
			var currPos = (BlockCoordinates) _entity.KnownPosition;
			if (currPos == _lastPosition)
			{
				if (_stallTime++ > 100) return false;
			}
			else
			{
				_stallTime = 0;
				_lastPosition = currPos;
			}

			return _currentPath.HavePath();
		}

		public override void OnTick(Entity[] entities)
		{
			if (_currentPath.HavePath())
			{
				if (!_currentPath.GetNextTile(_entity, out var next))
				{
					return;
				}

				_entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, _entity.KnownPosition.Y, (float) next.Y + 0.5f));
				_entity.EntityDirection = Mob.ClampDegrees(_entity.EntityDirection);
				_entity.KnownPosition.HeadYaw = (float) _entity.EntityDirection;
				_entity.KnownPosition.Yaw = (float) _entity.EntityDirection;

				_entity.Controller.MoveForward(_speedMultiplier, entities);
			}
		}

		public override void OnEnd()
		{
			_entity.Velocity *= new Vector3(0, 1, 0);
			_currentPath = null;
			_stallTime = 0;
		}

		protected static BlockCoordinates? FindRandomTargetBlock(Entity entity, int dxz, int dy, Vector3? targetDirection = null)
		{
			Random random = entity.Level.Random;

			BlockCoordinates coords = (BlockCoordinates) entity.KnownPosition;
			if (targetDirection.HasValue)
			{
				coords = coords + (BlockCoordinates) (targetDirection.Value.Normalize() * (float) Math.Ceiling(dxz / 3f));
			}

			double currentWeight = double.MinValue;
			Block currentBlock = null;
			for (int i = 0; i < 10; i++)
			{
				int x = random.Next(2 * dxz + 1) - dxz;
				int y = random.Next(2 * dy + 1) - dy;
				int z = random.Next(2 * dxz + 1) - dxz;

				var blockCoordinates = new BlockCoordinates(x, y, z) + coords;
				var block = entity.Level.GetBlock(blockCoordinates);
				var blockDown = entity.Level.GetBlock(blockCoordinates.BlockDown());
				if (blockDown.IsSolid)
				{
					double weight = CalculateBlockWeight(entity, block, blockDown);
					if (weight > currentWeight)
					{
						currentWeight = weight;
						currentBlock = block;
					}
				}
			}

			return currentBlock?.Coordinates;
		}


		private static double CalculateBlockWeight(Entity entity, Block block, Block blockDown)
		{
			// Calculate the weight(attractiveness) of each block:
			// If the mob is an animal (except spiders):
			//		A grass block automatically has a weight of 10
			//		If it's not grass, it's weight is it's light value minus .5
			// If the mob isn't an animal:
			//		the weight is equal to the darkness

			if (entity is PassiveMob)
			{
				if (blockDown is Grass) return 10;
				return Math.Max(block.BlockLight, block.SkyLight) - 0.5;
			}

			if (entity is HostileMob)
			{
				return 0.5 - Math.Max(block.BlockLight, block.SkyLight);
			}


			return 0;
		}
	}
}