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
	public partial class McpeText : Package<McpeText>
	{
		public string sourceName; // = null;
        public string sourceThirdPartyName; // = null;
        public int sourcePlatform; // = null;
        public string platformChatId; // = null;
        public string message; // = null;
        public string xboxUserId; 

		partial void AfterEncode()
		{
			Write(false);
			ChatTypes chatType = (ChatTypes) type;
			switch (chatType)
			{
				case ChatTypes.Chat:
				case ChatTypes.Whisper:
				case ChatTypes.Announcement:
					Write(sourceName);
                    Write(sourceThirdPartyName);
                    WriteVarInt(sourcePlatform);
                    Write(message);
					break;
				case ChatTypes.Raw:
				case ChatTypes.Tip:
				case ChatTypes.System:
					Write(message);
					break;
				case ChatTypes.Popup:
				case ChatTypes.Translation:
				case ChatTypes.Jukeboxpopup:
					Write(message);
					// More stuff
					break;
			}
            Write(xboxUserId);
            Write(platformChatId);
		}

		public override void Reset()
		{
			type = 0;
			sourceName = null;
            sourcePlatform = 0;
            sourceThirdPartyName = null;
            platformChatId = null;
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
					sourceName = ReadString();
                    sourcePlatform = ReadVarInt();
                    sourceThirdPartyName = ReadString();
					message = ReadString();
					break;
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
                    for (uint i = 0; i < parameterCount; ++i)
                    {
                        ReadString(); //TODO: translation parameters
                    }
                    break;
			}
            xboxUserId = ReadString();
            platformChatId = ReadString();
		}
	}
}