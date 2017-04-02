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

			Player player = _entity.Level.GetSpawnedPlayers().OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition.ToVector3()))
				.FirstOrDefault(p => Vector3.Distance(_entity.KnownPosition, p.KnownPosition) < _lookDistance && p.Inventory.GetItemInHand()?.GetType() == _temptingItem);

			if (player == null) return false;

			_player = player;

			return true;
		}

		public bool CanContinue()
		{
			return ShouldStart();
		}

		public void OnTick()
		{
		}

		private Tile? _lastTile = null;
		private List<Tile> _currentPath = null;

		public void CalculateNextMove()
		{
			if (_player == null) return;
			var distanceToPlayer = _entity.KnownPosition.DistanceTo(_player.KnownPosition);

			if (distanceToPlayer < 1.75)
			{
				// if within 6m stop following (walking)
				_entity.Velocity = Vector3.Zero;
				_entity.Controller.LookAt(_player, true);
				return;
			}

			BlockCoordinates currPos = (BlockCoordinates) _entity.KnownPosition;
			if (_currentPath == null || !_lastTile.HasValue || (((int) _lastTile.Value.X != currPos.X && (int) _lastTile.Value.Y != currPos.Z) && _currentPath.Count == 0))
			{
				Log.Debug($"Search new solution");
				var pathFinder = new PathFinder();
				_currentPath = pathFinder.FindPath(_entity, _player, distanceToPlayer + 1);
				//foreach (var next in _currentPath)
				//{
				//	//Log.Debug($"Steps to: {next.X}, {next.Y}");
				//	var level = _entity.Level;
				//	Block block = pathFinder.GetBlock(next);
				//	var particle = new RedstoneParticle(level);
				//	particle.Position = (Vector3) block.Coordinates + new Vector3(0.5f, 0.5f, 0.5f);
				//	particle.Spawn();
				//}
			}

			if (_currentPath.Count > 0)
			{
				Tile next = _currentPath.FirstOrDefault();
				if ((int) next.X == currPos.X && (int) next.Y == currPos.Z)
				{
					if (_currentPath.Count < 2)
					{
						_currentPath = null;
						_lastTile = null;
						Log.Warn($"Skipping one tick because no path");
						return;
					}
					//Log.Warn($"Using second tile in solution");
					_currentPath.Remove(next);
					next = _currentPath.FirstOrDefault();
				}

				_lastTile = next;

				_entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, _entity.KnownPosition.Y, (float) next.Y + 0.5f));
				//Log.Warn($"Rotate to: {next.X}, {next.Y}, {_entity.KnownPosition.Yaw}");


				if (distanceToPlayer < 1.75)
				{
					// if within 6m stop following (walking)
					_entity.Velocity = Vector3.Zero;
				}
				else
				{
					// else find path to player

					var m = 2.5 - distanceToPlayer;
					if (m <= 0)
					{
						m = 1;
					}
					else
					{
						m = m / 2.5;
					}
					//double m = 1;
					_entity.Controller.MoveForward(_speedMultiplier*m);
				}
			}
			else
			{
				Log.Warn($"Found no path solution");
				_currentPath = null;
				_lastTile = null;

				_entity.Velocity = Vector3.Zero;
			}

			_entity.Controller.LookAt(_player, true);
		}

		private void GetNext(BlockCoordinates currPos)
		{
			bool generate = _currentPath == null;
			generate = generate || _currentPath.Count == 0;
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
		}
	}
}