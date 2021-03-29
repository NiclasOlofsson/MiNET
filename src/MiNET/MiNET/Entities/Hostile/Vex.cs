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

using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Vex : HostileMob
	{
		public Vex(Level level) : base(EntityType.Vex, level)
		{
			Width = Length = 0.4;
			Height = 0.8;
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = new MetadataDictionary();
			metadata[0] = new MetadataLong(4398046511104); // 1000000000000000000000000000000000000000000; 
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
			metadata[54] = new MetadataFloat(0.4f);
			metadata[55] = new MetadataFloat(0.8f);
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

			return metadata;
		}
	}
}