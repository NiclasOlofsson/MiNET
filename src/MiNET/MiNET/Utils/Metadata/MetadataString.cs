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

using System;
using System.IO;
using System.Text;
using log4net;

namespace MiNET.Utils.Metadata
{
	public class MetadataString : MetadataEntry
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetadataString));

		public override byte Identifier
		{
			get { return 4; }
		}

		public override string FriendlyName
		{
			get { return "string"; }
		}

		public string Value { get; set; }

		public static implicit operator MetadataString(string value)
		{
			return new MetadataString(value);
		}

		public MetadataString()
		{
		}

		public MetadataString(string value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			try
			{
				var len = VarInt.ReadInt32(reader.BaseStream);

				byte[] bytes = new byte[len];
				reader.BaseStream.Read(bytes, 0, len);
				Value = Encoding.UTF8.GetString(bytes);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		public override void WriteTo(BinaryWriter stream)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(Value);
			VarInt.WriteInt32(stream.BaseStream, bytes.Length);

			stream.Write(bytes);
		}

		public override string ToString()
		{
			return string.Format("({0}) '{2}'", FriendlyName, Identifier, Value);
		}
	}
}