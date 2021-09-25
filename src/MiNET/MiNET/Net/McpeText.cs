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
	public partial class McpeText : Packet<McpeText>
	{
		public bool needsTranslation; // = null
		public string source; // = null;
		public string message; // = null;
		public string xuid; // = null
		public string platformChatId; // = null
		public string[] parameters; // = null

		partial void AfterEncode()
		{
			Write(needsTranslation);
			ChatTypes chatType = (ChatTypes) type;
			switch (chatType)
			{
				case ChatTypes.Chat:
				case ChatTypes.Whisper:
				case ChatTypes.Announcement:
					Write(source);
					goto case ChatTypes.Raw;
				case ChatTypes.Raw:
				case ChatTypes.Tip:
				case ChatTypes.System:
				case ChatTypes.Json:
					Write(message);
					break;
				case ChatTypes.Popup:
				case ChatTypes.Translation:
				case ChatTypes.Jukeboxpopup:
					Write(message);
					if (parameters == null)
					{
						WriteUnsignedVarInt(0);
					}
					else
					{
						WriteUnsignedVarInt((uint) parameters.Length);
						foreach (var parameter in parameters)
						{
							Write(parameter);
						}
					}
					break;
			}

			Write(xuid);
			Write(platformChatId);
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
			needsTranslation = ReadBool();

			ChatTypes chatType = (ChatTypes) type;
			switch (chatType)
			{
				case ChatTypes.Chat:
				case ChatTypes.Whisper:
				case ChatTypes.Announcement:
					source = ReadString();
					message = ReadString();
					break;
				case ChatTypes.Raw:
				case ChatTypes.Tip:
				case ChatTypes.System:
				case ChatTypes.Json:
				case ChatTypes.Jsonwhisper:
					message = ReadString();
					break;

				case ChatTypes.Popup:
				case ChatTypes.Translation:
				case ChatTypes.Jukeboxpopup:
					message = ReadString();
					parameters = new string[ReadUnsignedVarInt()];
					for (var i = 0; i < parameters.Length; ++i)
					{
						parameters[i] = ReadString();
					}
					break;
			}

			xuid = ReadString();
			platformChatId = ReadString();
		}
	}
}