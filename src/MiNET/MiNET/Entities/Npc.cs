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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Npc : Mob
	{
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

		public Npc(Level level) : base(EntityType.Npc, level)
		{
			Height = 2.1f;
			Width = 0.6f;
			Length = 0.6f;

			IsAlwaysShowName = true;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[41] = new MetadataString($"npc_{(int) NpcSkinType}");
			return metadata;
		}
	}
}