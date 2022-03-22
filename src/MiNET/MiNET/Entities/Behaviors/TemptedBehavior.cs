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
using System.Linq;
using System.Numerics;
using log4net;

namespace MiNET.Entities.Behaviors
{
	public class TemptedBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(TemptedBehavior));

		private readonly Mob _entity;
		private readonly Type _temptingItem;
		private readonly double _lookDistance;
		private readonly double _speedMultiplier;
		private Player _temptingPlayer;
		private int _cooldown = 0;
		private Vector3 _lastPlayerPos;
		private Vector3 _originalPos;
		private Path _currentPath = null;
		private Pathfinder _pathfinder = new Pathfinder();

		public TemptedBehavior(Mob entity, Type temptingItem, double lookDistance, double speedMultiplier)
		{
			this._entity = entity;
			_temptingItem = temptingItem;
			_lookDistance = lookDistance;
			_speedMultiplier = speedMultiplier;
		}

		public override bool ShouldStart()
		{
			if (_cooldown > 0)
			{
				_cooldown--;
				return false;
			}

			Player player = _entity.Level.GetSpawnedPlayers().OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition))
				.FirstOrDefault(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition) < _lookDistance && p.Inventory.GetItemInHand()?.GetType() == _temptingItem);

			if (player == null) return false;

			if (player != _temptingPlayer)
			{
				_temptingPlayer = player;
				_lastPlayerPos = _temptingPlayer.KnownPosition;
				_originalPos = _entity.KnownPosition;
			}

			return true;
		}

		public override bool CanContinue()
		{
			if (Math.Abs(_originalPos.Y - _entity.KnownPosition.Y) < 0.5)
				return ShouldStart();

			return false;
		}

		public override void OnTick(Entity[] entities)
		{
			if (_temptingPlayer == null) return;
			var distanceToPlayer = _entity.DistanceTo(_temptingPlayer);

			if (distanceToPlayer < 1.75)
			{
				_entity.Velocity = Vector3.Zero;
				_entity.Controller.LookAt(_temptingPlayer);

				_entity.Controller.RotateTowards(_temptingPlayer.KnownPosition);
				_entity.EntityDirection = Mob.ClampDegrees(_entity.EntityDirection);
				_entity.KnownPosition.HeadYaw = (float) _entity.EntityDirection;
				_entity.KnownPosition.Yaw = (float) _entity.EntityDirection;
				_currentPath = null;

				return;
			}

			bool haveNoPath = (_currentPath == null || _currentPath.NoPath());
			var deltaDistance = Vector3.Distance(_lastPlayerPos, _temptingPlayer.KnownPosition);
			if (haveNoPath || deltaDistance > 1)
			{
				_pathfinder = new Pathfinder();
				_currentPath = _pathfinder.FindPath(_entity, _temptingPlayer, _lookDistance);
				_lastPlayerPos = _temptingPlayer.KnownPosition;
			}


			if (_currentPath.HavePath())
			{
				if (!_currentPath.GetNextTile(_entity, out var next, true))
				{
					_currentPath = null;
					return;
				}

				_entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, _entity.KnownPosition.Y, (float) next.Y + 0.5f));
				_entity.EntityDirection = Mob.ClampDegrees(_entity.EntityDirection);
				_entity.KnownPosition.HeadYaw = (float) _entity.EntityDirection;
				_entity.KnownPosition.Yaw = (float) _entity.EntityDirection;

				if (distanceToPlayer < 1.75)
				{
					// if within x m stop following (walking)
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
				_entity.Velocity = Vector3.Zero;
				_currentPath = null;
			}

			_entity.Controller.LookAt(_temptingPlayer);
		}

		public override void OnEnd()
		{
			_cooldown = 100;
			_entity.Velocity = Vector3.Zero;
			_temptingPlayer = null;
			_entity.KnownPosition.Pitch = 0;
			_currentPath = null;
		}
	}
}