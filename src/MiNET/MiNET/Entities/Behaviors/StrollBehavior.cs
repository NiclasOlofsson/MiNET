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
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.Behaviors
{
	public class StrollBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(StrollBehavior));

		private readonly Mob _entity;
		private int _duration;
		private double _speed;
		private double _speedMultiplier;
		private int _timeLeft;

		public StrollBehavior(Mob entity, int duration, double speed, double speedMultiplier)
		{
			this._entity = entity;
			_duration = duration;
			_speed = speed;
			_speedMultiplier = speedMultiplier;
			_timeLeft = duration;
		}

		public override bool ShouldStart()
		{
			return _entity.Level.Random.Next(120) == 0;
		}

		public override bool CanContinue()
		{
			return _timeLeft-- > 0;
		}

		public override void OnTick(Entity[] entities)
		{
			float speedFactor = (float) (_speed * _speedMultiplier * 0.7f * (_entity.IsInWater ? 0.3 : 1.0)); // 0.7 is a general mob base factor
			var level = _entity.Level;
			var coordinates = _entity.KnownPosition;
			var direction = _entity.GetHorizDir() * new Vector3(1, 0, 1);

			var blockDown = level.GetBlock(((BlockCoordinates) coordinates).BlockDown());
			if (_entity.Velocity.Y < 0 && blockDown is Air)
			{
				_timeLeft = 0;
				return;
			}

			BlockCoordinates coord = (BlockCoordinates) (coordinates + (direction * speedFactor) + (direction * (float) _entity.Length / 2));

			var players = level.GetSpawnedPlayers();
			bool entityCollide = false;
			var boundingBox = _entity.GetBoundingBox().OffsetBy((direction * speedFactor) + (direction * (float) _entity.Length / 2));
			foreach (var player in players)
			{
				if (player.GetBoundingBox().Intersects(boundingBox))
				{
					entityCollide = true;
					break;
				}
			}

			if (!entityCollide)
			{
				var bbox = boundingBox;
				foreach (var ent in entities)
				{
					if (ent == _entity) continue;

					if (ent.EntityId > _entity.EntityId && _entity.IsColliding(bbox, ent))
					{
						if (_entity.Velocity == Vector3.Zero && level.Random.Next(1000) == 0)
						{
							break;
						}
						entityCollide = true;
						break;
					}
				}
			}

			var block = level.GetBlock(coord);
			var blockUp = level.GetBlock(coord.BlockUp());
			var blockUpUp = level.GetBlock(coord.BlockUp().BlockUp());

			var colliding = block.IsSolid || (_entity.Height >= 1 && blockUp.IsSolid);
			if (!colliding && !entityCollide)
			{
				var velocity = direction * speedFactor;
				//Log.Debug($"Moving sheep: {velocity}");
				if ((_entity.Velocity * new Vector3(1, 0, 1)).Length() < velocity.Length())
				{
					_entity.Velocity += velocity - _entity.Velocity;
				}
				else
				{
					_entity.Velocity = velocity;
				}
			}
			else
			{
				if (!entityCollide && !blockUp.IsSolid && !(_entity.Height > 1 && blockUpUp.IsSolid) && level.Random.Next(4) != 0)
				{
					//Log.Debug($"Block ahead: {block}, jumping");
					_entity.Velocity = new Vector3(0, 0.42f, 0);
				}
				else
				{
					//Log.Debug($"Block ahead: {block}, turning");
					int rot = level.Random.Next(2) == 0 ? level.Random.Next(45, 180) : level.Random.Next(-180, -45);
					_entity.EntityDirection += rot;
					_entity.EntityDirection = Mob.ClampDegrees(_entity.EntityDirection);
					_entity.KnownPosition.HeadYaw = (float) _entity.EntityDirection;
					_entity.KnownPosition.Yaw = (float) _entity.EntityDirection;
					_entity.Velocity *= new Vector3(0, 1, 0);
				}
			}
		}

		public override void OnEnd()
		{
			_timeLeft = _duration;
			_entity.Velocity *= new Vector3(0, 1, 0);
		}

		private bool AreaIsClear(Level level, BoundingBox bbox)
		{
			BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x < max.X; x++)
			{
				for (int y = min.Y; y < max.Y; y++)
				{
					for (int z = min.Z; z < max.Z; z++)
					{
						if (!level.IsAir(new BlockCoordinates(x, y, z))) return false;
					}
				}
			}

			return true;
		}
	}
}