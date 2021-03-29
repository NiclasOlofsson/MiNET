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
using MiNET.Utils.Vectors;

namespace MiNET.Net
{
	public partial class McpeCommandBlockUpdate : Packet<McpeCommandBlockUpdate>
	{
		public BlockCoordinates coordinates; // = null;
		public uint commandBlockMode; // = null;
		public bool isRedstoneMode; // = null;
		public bool isConditional; // = null;
		public long minecartEntityId; // = null;
		public string command; // = null;
		public string lastOutput; // = null;
		public string name; // = null;
		public bool shouldTrackOutput; // = null;

		partial void AfterEncode()
		{
			if (isBlock)
			{
				Write(coordinates);
				WriteUnsignedVarInt(commandBlockMode);
				Write(isRedstoneMode);
				Write(isConditional);
			}
			else
			{
				WriteUnsignedVarLong(minecartEntityId);
			}

			Write(command);
			Write(lastOutput);
			Write(name);
			Write(shouldTrackOutput);
		}

		partial void AfterDecode()
		{
			if (isBlock)
			{
				coordinates = ReadBlockCoordinates();
				commandBlockMode = ReadUnsignedVarInt();
				isRedstoneMode = ReadBool();
				isConditional = ReadBool();
			}
			else
			{
				minecartEntityId = ReadUnsignedVarLong();
			}

			command = ReadString();
			lastOutput = ReadString();
			name = ReadString();
			shouldTrackOutput = ReadBool();
		}

		public override void Reset()
		{
			coordinates = default;
			commandBlockMode = default;
			isRedstoneMode = default;
			isConditional = default;
			minecartEntityId = default;
			command = default;
			lastOutput = default;
			name = default;
			shouldTrackOutput = default;

			base.Reset();
		}
	}
}