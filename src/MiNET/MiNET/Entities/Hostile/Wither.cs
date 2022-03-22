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
using log4net;
using MiNET.Utils;
using MiNET.Utils.IO;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Wither : HostileMob, IAgeable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ElderGuardian));

		public int AnimationStep { get; set; } = 0;
		public bool ShowAuora { get; set; } = true;

		public Wither(Level level) : base(EntityType.Wither, level)
		{
			Width = Length = 1;
			Height = 3.5;
			HealthManager.MaxHealth = 400;
			HealthManager.Health = 4;
			//HealthManager.ResetHealth();
			NoAi = true;
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataLong(8592556032); // 1000000000001010000000000000000000; CanClimb, CanFly, Breathing
			metadata[1] = new MetadataInt(1);
			metadata[2] = new MetadataInt(0);
			metadata[3] = new MetadataByte(0);
			metadata[4] = new MetadataString("");
			metadata[5] = new MetadataLong(-1);
			metadata[7] = new MetadataShort(300);
			metadata[8] = new MetadataInt(0);
			metadata[9] = new MetadataByte(0);
			metadata[10] = new MetadataByte(0);
			metadata[22] = new MetadataByte(0);
			metadata[38] = new MetadataLong(0);
			metadata[39] = new MetadataFloat(1f);
			metadata[43] = new MetadataShort(300);
			metadata[44] = new MetadataInt(0);
			metadata[45] = new MetadataByte(0);
			metadata[46] = new MetadataInt(0);
			metadata[47] = new MetadataInt(0);
			metadata[49] = new MetadataInt(AnimationStep);
			metadata[50] = new MetadataLong(-1);
			metadata[51] = new MetadataLong(-1);
			metadata[52] = new MetadataLong(-1);
			metadata[53] = new MetadataShort((short) (ShowAuora ? 0 : 1));
			metadata[54] = new MetadataFloat(1f);
			metadata[55] = new MetadataFloat(3f);
			metadata[58] = new MetadataByte(0);
			metadata[59] = new MetadataFloat(0f);
			metadata[60] = new MetadataFloat(0f);
			metadata[70] = new MetadataByte(0);
			metadata[71] = new MetadataString("");
			metadata[72] = new MetadataString("");
			metadata[73] = new MetadataByte(1);
			metadata[74] = new MetadataByte(0);
			metadata[75] = new MetadataInt(0);
			metadata[76] = new MetadataInt(0);
			metadata[77] = new MetadataInt(0);
			metadata[78] = new MetadataInt(-1);

			//metadata[49] = new MetadataInt(0);
			//metadata[53] = new MetadataShort(0);

			return metadata;
		}

		private long _tick = 0;
		private readonly CooldownTimer _cooldown = new CooldownTimer(TimeSpan.FromMilliseconds(10000));

		public override void OnTick(Entity[] entities)
		{
			//base.OnTick();

			if (_cooldown.Execute())
			{
				_tick = 2;
				ShowAuora = false;
				BroadcastSetEntityData();
				//ShowAuora = false;
				//BroadcastSetEntityData();
			}

			if (_tick-- >= 0)
			{
				if (_tick == 1)
				{
					ShowAuora = true;
				}
				else
				{
					ShowAuora = false;
				}

				AnimationStep = (int) _tick;
				BroadcastSetEntityData();
			}
		}
	}
}