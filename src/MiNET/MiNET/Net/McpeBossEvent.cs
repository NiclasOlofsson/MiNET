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

namespace MiNET.Net
{
	public partial class McpeBossEvent
	{
		public ushort unknown6;

		partial void AfterEncode()
		{
			switch (eventType)
			{
				case BossEvent.Add:
					Write(bossName);
					Write(bossHealthPercent);
					Write(darkenScreen);
					WriteUnsignedVarInt(color);
					WriteUnsignedVarInt(overlay);
					break;
				case BossEvent.PlayerAdded:
				case BossEvent.PlayerRemoved:
					WriteSignedVarLong(playerId);
					break;
				case BossEvent.UpdatePercent:
					Write(bossHealthPercent);
					break;
				case BossEvent.UpdateName:
					Write(bossName);
					break;
				case BossEvent.UpdateProperties:
					Write(darkenScreen);
					goto case BossEvent.UpdateStyle;
				case BossEvent.UpdateStyle:
					WriteUnsignedVarInt(color);
					WriteUnsignedVarInt(overlay);
					break;
			}
		}

		public override void Reset()
		{
			base.Reset();
		}

		partial void AfterDecode()
		{
			switch (eventType)
			{
				case BossEvent.Add:
					bossName = ReadString();
					bossHealthPercent = ReadFloat();
					darkenScreen = ReadUshort();
					color = ReadUnsignedVarInt();
					overlay = ReadUnsignedVarInt();
					break;
				case BossEvent.PlayerAdded:
				case BossEvent.PlayerRemoved:
					playerId = ReadSignedVarLong();
					break;
				case BossEvent.UpdatePercent:
					bossHealthPercent = ReadFloat();
					break;
				case BossEvent.UpdateName:
					bossName = ReadString();
					break;
				case BossEvent.UpdateProperties:
					darkenScreen = ReadUshort();
					goto case BossEvent.UpdateStyle;
				case BossEvent.UpdateStyle:
					color = ReadUnsignedVarInt();
					overlay = ReadUnsignedVarInt();
					break;
			}
		}
	}
}