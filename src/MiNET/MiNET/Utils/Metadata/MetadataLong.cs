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
	public class MetadataLong : MetadataEntry
	{
		public byte id = 7;

		public override byte Identifier
		{
			get { return id; }
		}

		public override string FriendlyName
		{
			get { return "long"; }
		}

		public long Value { get; set; }

		public static implicit operator MetadataLong(long value)
		{
			return new MetadataLong(value);
		}

		public MetadataLong()
		{
		}

		public MetadataLong(long value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = VarInt.ReadSInt64(reader.BaseStream);
		}

		public override void WriteTo(BinaryWriter stream)
		{
			VarInt.WriteSInt64(stream.BaseStream, Value);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}