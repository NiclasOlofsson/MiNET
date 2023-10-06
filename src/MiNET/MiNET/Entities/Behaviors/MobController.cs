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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Entities.Behaviors
{
	public class MobController
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MobController));

		private readonly Mob _entity;

		public MobController(Mob entity)
		{
			_entity = entity;
		}

		public void LookAt(Entity target)
		{
			if (target == null)
			{
				_entity.KnownPosition.Pitch = 0;

				return;
			}

			Vector3 targetPos = target.KnownPosition + new Vector3(0, (float) (target is Player ? 1.62f : target.Height), 0);
			Vector3 entityPos = _entity.KnownPosition + new Vector3(0, (float) _entity.Height, 0) + _entity.GetHorizDir() * (float) _entity.Length / 2f;
			var d = Vector3.Normalize(targetPos - entityPos);

			var dx = d.X;
			var dy = d.Y;
			var dz = d.Z;

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx / (dz)));
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

			double tanOutput = 90 - RadianToDegree(Math.Atan(dx / (dz)));
			double thetaOffset = 270d;
			if (dz < 0)
			{
				thetaOffset = 90;
			}
			var yaw = /*ClampDegrees*/ (thetaOffset + tanOutput);

			_entity.EntityDirection = (float) yaw;
		}


		private double ClampDegrees(double degrees)
		{
			return Math.Floor((degrees % 360 + 360) % 360);
		}

		private int _jumpCooldown = 0;

		public void MoveForward(double speedMultiplier, Entity[] entities)
		{
			if (_jumpCooldown > 0)
			{
				_jumpCooldown--;
				//return;
			}

			float speedFactor = (float) (_entity.Speed * speedMultiplier * 0.7f);
			var level = _entity.Level;
			var currPosition = _entity.KnownPosition;
			var direction = Vector3.Normalize(_entity.GetHorizDir() * new Vector3(1, 0, 1));

			bool entityCollide = false;
			var boundingBox = _entity.GetBoundingBox().OffsetBy(direction * speedFactor);

			var players = level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				var bbox = boundingBox + 0.15f;
				if (player.GetBoundingBox().Intersects(bbox))
				{
					entityCollide = true;
					break;
				}
			}

			if (!_entity.IsPanicking && !entityCollide)
			{
				var bbox = boundingBox + 0.3f;
				foreach (var ent in entities)
				{
					if (ent == _entity) continue;

					if (ent.EntityId < _entity.EntityId && _entity.IsColliding(bbox, ent))
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

			Vector3 futurePosition = (currPosition + (direction * speedFactor) + (direction * (float) (_entity.Length * 0.5f)));
			var coord = (BlockCoordinates) futurePosition;
			var block = level.GetBlock(coord);
			var blockUp = level.GetBlock(coord.BlockUp());
			var blockUpUp = level.GetBlock(coord.BlockUp().BlockUp());

			//TODO: Need to rewrite so that the question is "is colliding with block" not "is solid block"
			var blockCollide = IsCollidingWithBlock(futurePosition, block) || (_entity.Height > 1 && BlockIsSolid(blockUp));

			if (!blockCollide && !entityCollide)
			{
				var blockDown = level.GetBlock(coord.BlockDown());
				if (!_entity.IsOnGround && !BlockIsSolid(blockDown)) return;

				//Log.Debug($"Move forward: {block}, {(_entity.IsOnGround ? "On ground" : "not on ground")}, Position: {(Vector3) _entity.KnownPosition}");
				//if (!_entity.IsOnGround) return;

				var velocity = direction * speedFactor;
				//Log.Debug($"Moving sheep ({_entity.KnownPosition.Yaw}: {velocity}, {_entity.Velocity}");
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
				if (_entity.CanClimb && !entityCollide)
				{
					_entity.Velocity = new Vector3(0, 0.2f, 0);
				}
				else if (!entityCollide && !BlockIsSolid(blockUp) && !(_entity.Height > 1 && BlockIsSolid(blockUpUp)) /*&& level.Random.Next(4) != 0*/)
				{
					// Above is wrong. Checks the wrong block in the wrong way.

					//Log.Debug($"Block ahead: {block}, {(_entity.IsOnGround ? "jumping" : "no jump")}, Position: {(Vector3)_entity.KnownPosition}");
					//_entity.Level.SetBlock(new StainedGlass() {Coordinates = block.Coordinates, Metadata = (byte) _entity.Level.Random.Next(16)});

					if (_entity.IsOnGround && _jumpCooldown <= 0)
					{
						_jumpCooldown = 10;
						_entity.Velocity += new Vector3(0, 0.42f, 0);
					}
				}
				else
				{
					if (entityCollide)
					{
						//Log.Debug($"Entity ahead: {block}, stopping");
						_entity.Velocity *= new Vector3(0, 1, 0);
					}
					else
					{
						//Log.Debug($"Block ahead: {block}, ignoring");
						//_entity.Level.SetBlock(new StainedGlass() { Coordinates = block.Coordinates + BlockCoordinates.Down, Metadata = (byte)_entity.Level.Random.Next(16) });
						_entity.Velocity *= new Vector3(0, 1, 0);
					}
				}
			}
		}

		private static bool IsCollidingWithBlock(Vector3 pos, Block block)
		{
			if (block is DoorBase door)
			{
				return !door.OpenBit;
			}

			if (block.IsSolid)
			{
				BoundingBox bbox = block.GetBoundingBox();
				return pos.Y - bbox.Max.Y < 0;
			}

			return false;
		}

		private static bool BlockIsSolid(Block block)
		{
			if (block is DoorBase door)
			{
				return !door.OpenBit;
			}

			return block.IsSolid;
		}

		public void Jump()
		{
		}

		private double RadianToDegree(double angle)
		{
			return angle * (180.0 / Math.PI);
		}
	}
}