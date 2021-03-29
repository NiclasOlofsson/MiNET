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

using System.IO;

namespace MiNET.Utils.Metadata
{
	public abstract class MetadataEntry
	{
		public abstract byte Identifier { get; }
		public abstract string FriendlyName { get; }

		public abstract void FromStream(BinaryReader reader);
		public abstract void WriteTo(BinaryWriter stream);

		internal byte Index { get; set; }

		public static implicit operator MetadataEntry(byte value)
		{
			return new MetadataByte(value);
		}

		public static implicit operator MetadataEntry(short value)
		{
			return new MetadataShort(value);
		}

		public static implicit operator MetadataEntry(int value)
		{
			return new MetadataInt(value);
		}

		public static implicit operator MetadataEntry(float value)
		{
			return new MetadataFloat(value);
		}

		public static implicit operator MetadataEntry(string value)
		{
			return new MetadataString(value);
		}

		public static implicit operator MetadataEntry(long value)
		{
			return new MetadataLong(value);
		}
	}
}