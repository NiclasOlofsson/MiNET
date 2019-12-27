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
		public string title;
		public float healthPercent;
		public long playerId;
		public uint color = 0xff00ff00;
		public uint overlay = 0xff00ff00;

		partial void AfterEncode()
		{
			switch ((McpeBossEvent.Type)eventType)
			{
				case Type.AddPlayer:
				case Type.RemovePlayer:
					WriteSignedVarLong(playerId);
					break;

				case Type.UpdateProgress:
					Write(healthPercent);
					break;

				case Type.UpdateName:
					Write(title);
					break;

				case Type.AddBoss:
					Write(title);
					Write(healthPercent);
					goto case Type.UpdateOptions;
				case Type.UpdateOptions:
					Write(unknown6);
					goto case Type.UpdateStyle;
				case Type.UpdateStyle:
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
			switch ((McpeBossEvent.Type) eventType)
			{
				case Type.AddPlayer:
				case Type.RemovePlayer:
					// Entity Unique ID
					ReadSignedVarLong();
					break;
				case Type.UpdateProgress:
					// float
					ReadFloat();
					break;
				case Type.UpdateName:
					// string
					ReadString();
					break;
				case Type.AddBoss:
					// string
					ReadString();
					// float
					ReadFloat();
					goto case Type.UpdateOptions;
				case Type.UpdateOptions:
					// ushort?
					ReadShort();
					goto case Type.UpdateStyle;
				case Type.UpdateStyle:
					// NOOP
					ReadUnsignedVarInt();
					ReadUnsignedVarInt();
					break;
			}
		}
	}
}