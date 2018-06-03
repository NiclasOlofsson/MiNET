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
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Npc : Mob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Npc));

		public enum NpcTypes
		{
			Npc1 = 1,
			Npc2,
			Npc3,
			Npc4,
			Npc5,
			Npc6,
			Npc7,
			Npc8,
			Npc9,
			Npc10,
		}

		public NpcTypes NpcSkinType { get; set; } = NpcTypes.Npc1;
		public string DialogText { get; set; } = "";
		public string AdvancedConfig { get; set; } = "";

		public Npc(Level level) : base(EntityType.Npc, level)
		{
			Height = 2.1f;
			Width = 0.6f;
			Length = 0.6f;

			CanDespawn = false;
			IsAlwaysShowName = true;
		}

		public override void SetEntityData(MetadataDictionary metadata)
		{
			if (metadata.Contains(4))
			{
				var entry = metadata[4];
				var s = entry as MetadataString;
				NameTag = s?.Value;
				Log.Debug($"NameTag={NameTag}");
			}
			if (metadata.Contains(40))
			{
				var entry = metadata[40];
				var s = entry as MetadataString;
				DialogText = s?.Value;
			}
			if (metadata.Contains(41))
			{
				var entry = metadata[41];
				var s = entry as MetadataString;
				string npcType = s?.Value;
				npcType = npcType.Substring(4);
				if (Enum.TryParse(npcType, true, out NpcTypes t))
				{
					NpcSkinType = t;
				}
			}
			if (metadata.Contains(42))
			{
				var entry = metadata[42];
				var s = entry as MetadataString;
				AdvancedConfig = s?.Value;
			}

			BroadcastSetEntityData();
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[40] = new MetadataString(DialogText);
			metadata[41] = new MetadataString($"npc_{(int) NpcSkinType}");
			metadata[42] = new MetadataString(AdvancedConfig);
			return metadata;
		}

		public override void OnTick(Entity[] entities)
		{
		}
	}
}