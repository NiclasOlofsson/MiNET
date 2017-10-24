﻿#region LICENSE

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
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class TemptedBehavior : IBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (TemptedBehavior));

		private readonly Mob _entity;
		private readonly Type _temptingItem;
		private readonly double _lookDistance;
		private readonly double _speedMultiplier;
		private Player _player;
		private int _cooldown = 0;
		private Vector3 _lastPlayerPos;

		public TemptedBehavior(Mob entity, Type temptingItem, double lookDistance, double speedMultiplier)
		{
			this._entity = entity;
			_temptingItem = temptingItem;
			_lookDistance = lookDistance;
			_speedMultiplier = speedMultiplier;
		}

		public bool ShouldStart()
		{
			if (_cooldown > 0)
			{
				_cooldown--;
				return false;
			}

			Player player = _entity.Level.GetSpawnedPlayers().OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition))
				.FirstOrDefault(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition) < _lookDistance && p.Inventory.GetItemInHand()?.GetType() == _temptingItem);

			if (player == null) return false;

			if (player != _player)
			{
				_player = player;
				_lastPlayerPos = _player.KnownPosition;
			}

			return true;
		}

		public bool CanContinue()
		{
			return ShouldStart();
		}

		private List<Tile> _currentPath = null;

		public void OnTick(Entity[] entities)
		{
			if (_player == null) return;
			var distanceToPlayer = _entity.DistanceTo(_player);

			if (distanceToPlayer < 1.75)
			{
				_entity.Velocity = Vector3.Zero;
				_entity.Controller.LookAt(_player);

				_entity.Controller.RotateTowards(_player.KnownPosition);
				_entity.Direction = Mob.ClampDegrees(_entity.Direction);
				_entity.KnownPosition.HeadYaw = (float)_entity.Direction;
				_entity.KnownPosition.Yaw = (float)_entity.Direction;
				_currentPath = null;

				return;
			}

			bool haveNoPath = (_currentPath == null || _currentPath.Count == 0);
			var deltaDistance = Vector3.Distance(_lastPlayerPos, _player.KnownPosition);
			if (haveNoPath || deltaDistance > 0)
			{
				Log.Debug($"Search new solution");
				var pathFinder = new PathFinder();
				_currentPath = pathFinder.FindPath(_entity, _player, distanceToPlayer + 1);
				if (_currentPath.Count == 0)
				{
					_currentPath = pathFinder.FindPath(_entity, _player, _lookDistance);
				}
			}

			_lastPlayerPos = _player.KnownPosition;

			if (_currentPath.Count > 0)
			{
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
						m = m/2.0;
					}
					//double m = 1;
					_entity.Controller.MoveForward(_speedMultiplier*m, entities);
				}
			}
			else
			{
				_entity.Velocity = Vector3.Zero;
				_currentPath = null;
			}

			_entity.Controller.LookAt(_player, true);
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

		public double Distance(Player player, Tile tile)
		{
			Vector2 pos1 = new Vector2(player.KnownPosition.X, player.KnownPosition.Z);
			Vector2 pos2 = new Vector2((float) tile.X, (float) tile.Y);
			return (pos1 - pos2).Length();
		}

		public void OnEnd()
		{
			_cooldown = 100;
			_entity.Velocity = Vector3.Zero;
			_player = null;
			_entity.KnownPosition.Pitch = 0;
			_currentPath = null;
		}
	}
}