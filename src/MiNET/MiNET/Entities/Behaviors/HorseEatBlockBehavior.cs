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
using MiNET.Blocks;
using MiNET.Entities.Passive;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Entities.Behaviors
{
	public class HorseEatBlockBehavior : BehaviorBase
	{
		private readonly Mob _entity;
		private int _duration;
		private int _timeLeft;

		public HorseEatBlockBehavior(Mob entity, int duration)
		{
			this._entity = entity;
			_duration = Math.Max(40, duration);
			_timeLeft = _duration;
		}

		public override bool ShouldStart()
		{
			if (!(_entity is Horse)) return false;

			if (_entity.Level.Random.Next(1000) != 0) return false;

			var coordinates = _entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			var shouldStart = _entity.Level.GetBlock(coord.BlockDown()) is Grass || _entity.Level.GetBlock(coord) is Tallgrass;
			if (!shouldStart) return false;

			_duration = 40;

			_entity.Velocity *= new Vector3(0, 1, 0);
			SetEating((Horse) _entity, true);

			return true;
		}

		public override bool CanContinue()
		{
			return _duration-- > 0;
		}

		public override void OnTick(Entity[] entities)
		{
		}

		public override void OnEnd()
		{
			var coordinates = _entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			Block broken = null;
			if (_entity.Level.GetBlock(coord) is Tallgrass)
			{
				broken = _entity.Level.GetBlock(coord);
				_entity.Level.SetAir(coord);
			}
			else
			{
				coord += BlockCoordinates.Down;
				broken = _entity.Level.GetBlock(coord);
				_entity.Level.SetBlock(new Dirt {Coordinates = coord});
			}
			DestroyBlockParticle particle = new DestroyBlockParticle(_entity.Level, broken);
			particle.Spawn();
			SetEating((Horse) _entity, false);
		}

		private void SetEating(Horse horse, bool isEating)
		{
			horse.IsEating = isEating;
			horse.BroadcastSetEntityData();
		}
	}
}