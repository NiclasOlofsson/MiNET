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
using System.Numerics;

namespace MiNET.Utils.Metadata
{
	public class MetadataVector3 : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 8; }
		}

		public override string FriendlyName
		{
			get { return "vector3"; }
		}

		public Vector3 Value { get; set; }

		public MetadataVector3()
		{
		}

		public MetadataVector3(float x, float y, float z) : this(new Vector3(x, y, z))
		{
		}

		public MetadataVector3(Vector3 value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = new Vector3
			{
				X = reader.ReadSingle(),
				Y = reader.ReadSingle(),
				Z = reader.ReadSingle(),
			};
		}

		public override void WriteTo(BinaryWriter reader)
		{
			reader.Write(Value.X);
			reader.Write(Value.Y);
			reader.Write(Value.Z);
		}

		public override string ToString()
		{
			return string.Format("({0}) {2}", FriendlyName, Identifier, Value);
		}
	}
}