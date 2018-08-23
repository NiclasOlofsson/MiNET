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
	public partial class McpeText : Packet<McpeText>
	{
		public string source; // = null;
		public string message; // = null;

		partial void AfterEncode()
		{
			Write(false);
			ChatTypes chatType = (ChatTypes) type;
			switch (chatType)
			{
				case ChatTypes.Chat:
				case ChatTypes.Whisper:
				case ChatTypes.Announcement:
					Write(source);
					Write(""); //TODO: third party name
					WriteSignedVarInt(0); //TODO: platform
					goto case ChatTypes.Raw;
				case ChatTypes.Raw:
				case ChatTypes.Tip:
				case ChatTypes.System:
					Write(message);
					break;
				case ChatTypes.Popup:
				case ChatTypes.Translation:
				case ChatTypes.Jukeboxpopup:
					Write(message);
					WriteUnsignedVarInt(0); //TODO: translation parameters (list of strings)
					break;
			}

			Write(""); //TODO: XUID
			Write(""); //TODO: platform chat ID
		}

		public override void Reset()
		{
			type = 0;
			source = null;
			message = null;

			base.Reset();
		}

		partial void AfterDecode()
		{
			ReadBool(); // localization

			ChatTypes chatType = (ChatTypes) type;
			switch (chatType)
			{
				case ChatTypes.Chat:
				case ChatTypes.Whisper:
				case ChatTypes.Announcement:
					source = ReadString();
					ReadString(); //TODO: third party name
					ReadSignedVarInt(); //TODO: platform
					goto case ChatTypes.Raw;
				case ChatTypes.Raw:
				case ChatTypes.Tip:
				case ChatTypes.System:
					message = ReadString();
					break;

				case ChatTypes.Popup:
				case ChatTypes.Translation:
				case ChatTypes.Jukeboxpopup:
					message = ReadString();
					uint parameterCount = ReadUnsignedVarInt();
					for(uint i = 0; i < parameterCount; ++i)
					{
						ReadString(); //TODO: translation parameters
					}
					// More stuff
					break;
			}

			ReadString(); //TODO: XUID
			ReadString(); //TODO: platform chat ID
		}
	}
}