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
using MiNET.Utils.Vectors;

namespace MiNET.Utils.Metadata
{
	public class MetadataIntCoordinates : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 6; }
		}

		public override string FriendlyName
		{
			get { return "int coordinates"; }
		}

		public BlockCoordinates Value { get; set; }

		public MetadataIntCoordinates()
		{
		}

		public MetadataIntCoordinates(int x, int y, int z)
		{
			Value = new BlockCoordinates(x, y, z);
		}

		public override void FromStream(BinaryReader reader)
		{
			Stream stream = reader.BaseStream;
			Value = new BlockCoordinates
			{
				X = VarInt.ReadSInt32(stream),
				Y = VarInt.ReadSInt32(stream),
				Z = VarInt.ReadSInt32(stream),
			};
		}

		public override void WriteTo(BinaryWriter reader)
		{
			Stream stream = reader.BaseStream;
			VarInt.WriteSInt32(stream, Value.X);
			VarInt.WriteSInt32(stream, Value.Y);
			VarInt.WriteSInt32(stream, Value.Z);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}