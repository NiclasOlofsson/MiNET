using System;
using System.Numerics;
using log4net;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class MobController
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MobController));

		private readonly Mob _entity;

		public MobController(Mob entity)
		{
			_entity = entity;
		}

		public void LookAt(Entity target, bool headOnly = true)
		{
			if (target == null)
			{
				_entity.KnownPosition.Pitch = 0;

				return;
			}

			Vector3 playerPosition = target.KnownPosition + new Vector3(0, (float) (target is Player ? 1.62f : target.Height), 0);
			Vector3 entityPosition = _entity.KnownPosition + new Vector3(0, (float) _entity.Height, 0) + _entity.GetHorizDir()*(float) _entity.Length/2f;
			var d = Vector3.Normalize(playerPosition - entityPosition);

			var dx = d.X;
			var dy = d.Y;
			var dz = d.Z;

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx/(dz)));
			double thetaOffset = 270d;
			if (dz < 0)
			{
				thetaOffset = 90;
			}
			var yaw = /*ClampDegrees*/ (thetaOffset + tanOutput);

			double pitch = RadianToDegree(-Math.Asin(dy));

			_entity.KnownPosition.Yaw = (float) yaw;
			_entity.KnownPosition.HeadYaw = (float) yaw;
			_entity.KnownPosition.Pitch = (float) pitch;
		}

		public void RotateTowards(Vector3 targetPosition)
		{
			Vector3 entityPosition = _entity.KnownPosition;
			var d = Vector3.Normalize(targetPosition - entityPosition);

			var dx = d.X;
			var dy = d.Y;
			var dz = d.Z;

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx/(dz)));
			double thetaOffset = 270d;
			if (dz < 0)
			{
				thetaOffset = 90;
			}
			var yaw = /*ClampDegrees*/ (thetaOffset + tanOutput);

			_entity.Direction = (float) yaw;
		}


		private double ClampDegrees(double degrees)
		{
			return Math.Floor((degrees%360 + 360)%360);
		}

		private int _jumpCooldown = 0;

		public void MoveForward(double speedMultiplier)
		{
			if (_jumpCooldown > 0)
			{
				_jumpCooldown--;
				return;
			}

			float speedFactor = (float) (_entity.Speed*speedMultiplier);
			var level = _entity.Level;
			var currPosition = _entity.KnownPosition;
			var direction = _entity.GetHorizDir()*new Vector3(1, 0, 1);

			var blockDown = level.GetBlock(currPosition + BlockCoordinates.Down);
			//if (_entity.Velocity.Y < 0 && !blockDown.IsSolid)
			//{
			//	Log.Debug($"Falling mob: {_entity.Velocity}, Position: {(Vector3)_entity.KnownPosition}");
			//	return;
			//}

			BlockCoordinates coord = (BlockCoordinates) (currPosition + (direction*speedFactor) + (direction*(float) _entity.Length/2));

			bool entityCollide = false;
			var boundingBox = _entity.GetBoundingBox().OffsetBy(direction*speedFactor);

			var players = level.GetSpawnedPlayers();
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
				var entities = level.GetEntites();
				foreach (var ent in entities)
				{
					if (ent == _entity) continue;

					if (ent.GetBoundingBox().Intersects(boundingBox) && ent.EntityId > _entity.EntityId)
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
			var blockUp = level.GetBlock(coord + BlockCoordinates.Up);
			var blockUpUp = level.GetBlock(coord + BlockCoordinates.Up + BlockCoordinates.Up);

			var colliding = block.IsSolid || (_entity.Height >= 1 && blockUp.IsSolid);
			if (!colliding && !entityCollide)
			{
				Log.Debug($"Move forward: {block}, {(_entity.IsOnGround ? "On ground" : "not on ground")}, Position: {(Vector3) _entity.KnownPosition}");
				//if (!_entity.IsOnGround) return;

				var velocity = direction*speedFactor;
				//Log.Debug($"Moving sheep ({_entity.KnownPosition.Yaw}: {velocity}, {_entity.Velocity}");
				if ((_entity.Velocity*new Vector3(1, 0, 1)).Length() < velocity.Length())
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
				if (!entityCollide && !blockUp.IsSolid && !(_entity.Height > 1 && blockUpUp.IsSolid) /*&& level.Random.Next(4) != 0*/)
				{
					Log.Debug($"Block ahead: {block}, {(_entity.IsOnGround ? "jumping" : "no jump")}, Position: {(Vector3) _entity.KnownPosition}");
					if (_entity.IsOnGround)
					{
						_jumpCooldown = 5;
						_entity.Velocity = new Vector3(0, 0.42f, 0);
					}
				}
				else
				{
					if (entityCollide)
					{
						Log.Debug($"Entity ahead: {block}, stopping");
						_entity.Velocity *= new Vector3(0, 1, 0);
					}
					else
					{
						Log.Debug($"Block ahead: {block}, ignoring");
						//var velocity = direction*speedFactor;
						//_entity.Velocity = velocity;
						_entity.Velocity *= new Vector3(0, 1, 0);
					}
				}
			}
		}

		public void Jump()
		{
		}

		private double RadianToDegree(double angle)
		{
			return angle*(180.0/Math.PI);
		}
	}
}