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
using AStarNavigator;
using log4net;
using MiNET.Entities.Passive;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Entities.Behaviors
{
	public class FollowOwnerBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TemptedBehavior));

		private readonly Wolf _entity;
		private readonly double _lookDistance;
		private readonly double _speedMultiplier;

		public FollowOwnerBehavior(Wolf entity, double lookDistance, double speedMultiplier)
		{
			_entity = entity;
			_lookDistance = lookDistance;
			_speedMultiplier = speedMultiplier;
		}

		public override bool ShouldStart()
		{
			if (!_entity.IsTamed) return false;
			if (_entity.Owner == null) return false;

			return true;
		}

		public override bool CanContinue()
		{
			return ShouldStart();
		}

		private Path _currentPath;

		public override void OnTick(Entity[] entities)
		{
			if (_entity.Owner == null) return;
			Player owner = (Player) _entity.Owner;

			var distanceToPlayer = _entity.DistanceTo(owner);

			if (distanceToPlayer < 1.75)
			{
				_entity.Velocity = Vector3.Zero;
				_entity.Controller.LookAt(owner);
				return;
			}

			if (_currentPath == null || _currentPath.NoPath())
			{
				Log.Debug($"Search new solution");
				var pathFinder = new Pathfinder();
				_currentPath = pathFinder.FindPath(_entity, owner, _lookDistance);
			}

			if (_currentPath.HavePath())
			{
				if (!_currentPath.GetNextTile(_entity, out Tile next)) return;

				_entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, _entity.KnownPosition.Y, (float) next.Y + 0.5f));

				if (distanceToPlayer < 1.75)
				{
					_entity.Velocity = Vector3.Zero;
					_currentPath = null;
				}
				else
				{
					// else find path to player

					var m = 2 - distanceToPlayer;
					if (m <= 0)
					{
						m = 1;
					}
					else
					{
						m = m / 2.0;
					}
					//double m = 1;
					_entity.Controller.MoveForward(_speedMultiplier * m, entities);
				}
			}
			else
			{
				Log.Debug($"Found no path solution");
				_entity.Velocity = Vector3.Zero;
				_currentPath = null;
			}

			_entity.Controller.LookAt(owner);
		}

		private bool GetNextTile(out Tile next)
		{
			next = null;
			if (_currentPath.NoPath()) return false;

			next = _currentPath.First();

			BlockCoordinates currPos = (BlockCoordinates) _entity.KnownPosition;
			if ((int) next.X == currPos.X && (int) next.Y == currPos.Z)
			{
				_currentPath.Remove(next);

				if (!GetNextTile(out next)) return false;
			}

			return true;
		}

		public double Distance(Player player, Tile tile)
		{
			Vector2 pos1 = new Vector2(player.KnownPosition.X, player.KnownPosition.Z);
			Vector2 pos2 = new Vector2((float) tile.X, (float) tile.Y);
			return (pos1 - pos2).Length();
		}

		public override void OnEnd()
		{
			_entity.Velocity = Vector3.Zero;
			_entity.KnownPosition.Pitch = 0;
			_currentPath = null;
		}
	}
}