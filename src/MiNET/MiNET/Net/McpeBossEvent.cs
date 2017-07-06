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

namespace MiNET.Net
{
	public partial class McpeBossEvent
	{
		public ushort unknown6;

		partial void AfterEncode()
		{
			switch (eventType)
			{
				case 1:
				case 3:
					// Entity Unique ID
					break;
				case 4:
					// float
					break;
				case 5:
					// string
					break;
				case 0:
					// string
					// float
					break;
				case 6:
					// ushort?
					Write(unknown6);
					goto case 7;
				case 7:
					// NOOP
					WriteUnsignedVarInt(0xff00ff00);
					WriteUnsignedVarInt(0xff00ff00);
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
				case 1:
				case 3:
					// Entity Unique ID
					ReadSignedVarLong();
					break;
				case 4:
					// float
					ReadFloat();
					break;
				case 5:
					// string
					ReadString();
					break;
				case 0:
					// string
					ReadString();
					// float
					ReadFloat();
					goto case 6;
				case 6:
					// ushort?
					ReadShort();
					goto case 7;
				case 7:
					// NOOP
					ReadUnsignedVarInt();
					ReadUnsignedVarInt();
					break;
			}
		}
	}
}