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

using System.Drawing;

namespace MiNET.Effects
{
	public class Absorption : Effect
	{
		public Absorption() : base(EffectType.Absorption)
		{
			ParticleColor = Color.FromArgb(0x25, 0x52, 0xA5);
		}

		public override void SendAdd(Player player)
		{
			base.SendAdd(player);
			player.HealthManager.Absorption = 4 * (Level + 1);
			player.SendUpdateAttributes();
		}

		public override void OnTick(Player player)
		{
			if (player.HealthManager.Absorption > 0)
			{
				base.OnTick(player);
			}
			else
			{
				player.RemoveEffect(this);
			}
		}

		public override void SendRemove(Player player)
		{
			base.SendRemove(player);
			player.HealthManager.Absorption = 0;
			player.SendUpdateAttributes();
		}
	}
}