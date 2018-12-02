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
using MiNET.Entities.Passive;

namespace MiNET.Entities.Behaviors
{
	public class SittingBehavior : BehaviorBase
	{
		private readonly Mob _entity;

		public SittingBehavior(Mob entity)
		{
			this._entity = entity;
		}

		public override bool ShouldStart()
		{
			if (!_entity.IsTamed) return false;
			if (_entity.IsInWater) return false;

			Player owner = ((Wolf) _entity).Owner as Player;

			var shouldStart = owner == null || ((!(_entity.KnownPosition.DistanceTo(owner.KnownPosition) < 144.0) || _entity.HealthManager.LastDamageSource == null) && _entity.IsSitting);
			if (!shouldStart) return false;

			_entity.Velocity *= new Vector3(0, 1, 0);

			return true;
		}

		public override bool CanContinue()
		{
			return _entity.IsSitting;
		}


		public override void OnTick(Entity[] entities)
		{
		}

		public override void OnEnd()
		{
			_entity.IsSitting = false;
			_entity.BroadcastSetEntityData();
		}
	}
}