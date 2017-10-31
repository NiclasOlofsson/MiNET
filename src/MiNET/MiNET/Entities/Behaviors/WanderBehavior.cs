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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AStarNavigator;
using log4net;
using MiNET.Blocks;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Particles;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class WanderBehavior : IBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (WanderBehavior));

		private readonly Mob _entity;
		private readonly double _speed;
		private readonly double _speedMultiplier;
		private readonly int _chance;
		private List<Tile> _currentPath;
		private Pathfinder _pathfinder;

		public WanderBehavior(Mob entity, double speed, double speedMultiplier, int chance = 120)
		{
			_entity = entity;
			_speed = speed;
			_speedMultiplier = speedMultiplier;
			_chance = chance;
		}

		public bool ShouldStart()
		{
			if (_entity.Level.Random.Next(_chance) != 0) return false;

			BlockCoordinates? pos = FindRandomTargetBlock(_entity, 10, 7);

			if (!pos.HasValue) return false;

			_pathfinder = new Pathfinder();
			_currentPath = _pathfinder.FindPath(_entity, pos.Value, pos.Value.DistanceTo((BlockCoordinates) _entity.KnownPosition) + 2);

			return _currentPath.Count > 0;
		}

		public bool CanContinue()
		{
			return _currentPath != null && _currentPath.Count > 0;
		}

		public void OnTick(Entity[] entities)
		{
			if (_currentPath.Count > 0)
			{
				// DEBUG
				_pathfinder.PrintPath(_entity.Level, _currentPath);

				Tile next;
				if (!GetNextTile(out next))
				{
					_currentPath = null;
					return;
				}

				_entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, _entity.KnownPosition.Y, (float) next.Y + 0.5f));
				_entity.Direction = Mob.ClampDegrees(_entity.Direction);
				_entity.KnownPosition.HeadYaw = (float) _entity.Direction;
				_entity.KnownPosition.Yaw = (float) _entity.Direction;

				_entity.Controller.MoveForward(_speedMultiplier, entities);
				if (_entity.Velocity.Length() < 0.01) _currentPath = null;
			}
			else
			{
				_currentPath = null;
			}
		}

		private bool GetNextTile(out Tile next)
		{
			next = new Tile();
			if (_currentPath.Count == 0) return false;

			next = _currentPath.First();

			BlockCoordinates currPos = (BlockCoordinates) _entity.KnownPosition;
			if ((int) next.X == currPos.X && (int) next.Y == currPos.Z)
			{
				_currentPath.Remove(next);

				if (!GetNextTile(out next)) return false;
			}

			return true;
		}

		public void OnEnd()
		{
			_entity.Velocity *= new Vector3(0, 1, 0);
		}

		private static BlockCoordinates? FindRandomTargetBlock(Entity entity, int dxz, int dy, Vector3? targetDirectinon = null)
		{
			Random random = entity.Level.Random;

			BlockCoordinates coords = (BlockCoordinates) entity.KnownPosition;

			double currentWeight = 0;
			Block currentBlock = null;
			for (int i = 0; i < 10; i++)
			{
				int x = random.Next(2*dxz + 1) - dxz;
				int y = random.Next(2*dy + 1) - dy;
				int z = random.Next(2*dxz + 1) - dxz;

				var blockCoordinates = new BlockCoordinates(x, y, z) + coords;
				var block = entity.Level.GetBlock(blockCoordinates);
				var blockDown = entity.Level.GetBlock(blockCoordinates + BlockCoordinates.Down);
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