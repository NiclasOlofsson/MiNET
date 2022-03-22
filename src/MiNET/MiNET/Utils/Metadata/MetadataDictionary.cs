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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

namespace MiNET.Utils.Metadata
{
	/// <summary>
	///     Used to send metadata with entities
	/// </summary>
	public class MetadataDictionary
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MetadataDictionary));

		public readonly Dictionary<int, MetadataEntry> _entries;

		public MetadataDictionary()
		{
			_entries = new Dictionary<int, MetadataEntry>();
		}

		public int Count
		{
			get { return _entries.Count; }
		}

		public MetadataEntry this[int index]
		{
			get { return _entries[index]; }
			set { _entries[index] = value; }
		}

		public MetadataEntry[] GetValues()
		{
			return _entries.Values.ToArray();
		}

		public bool Contains(byte index)
		{
			return _entries.ContainsKey(index);
		}

		public static MetadataDictionary FromStream(BinaryReader reader)
		{
			Stream stream = reader.BaseStream;
			MetadataDictionary metadata = new MetadataDictionary();

			{
				var count = VarInt.ReadInt32(stream);

				for (int i = 0; i < count; i++)
				{
					int index = VarInt.ReadInt32(stream);
					int type = VarInt.ReadInt32(stream);
					var entry = EntryTypes[type]();

					entry.FromStream(reader);
					entry.Index = (byte) index;

					metadata[index] = entry;
				}
			}

			return metadata;
		}

		public void WriteTo(BinaryWriter writer)
		{
			Stream stream = writer.BaseStream;

			VarInt.WriteInt32(stream, _entries.Count);
			foreach (var entry in _entries)
			{
				VarInt.WriteInt32(stream, entry.Key);
				VarInt.WriteInt32(stream, entry.Value.Identifier);
				entry.Value.WriteTo(writer);
			}
		}

		public delegate MetadataEntry CreateEntryInstance();

		public static readonly CreateEntryInstance[] EntryTypes = new CreateEntryInstance[]
		{
			() => new MetadataByte(), // 0
			() => new MetadataShort(), // 1
			() => new MetadataInt(), // 2
			() => new MetadataFloat(), // 3
			() => new MetadataString(), // 4
			() => new MetadataNbt(), // 5
			() => new MetadataIntCoordinates(), // 6
			() => new MetadataLong(), // 7
			() => new MetadataVector3(), // 8
		};

		public override string ToString()
		{
			StringBuilder sb = null;

			foreach (var entry in _entries)
			{
				if (sb != null)
					sb.Append(", ");
				else
					sb = new StringBuilder();

				sb.Append("[" + entry.Key + "]");
				sb.Append(entry.Value.ToString());
			}

			if (sb != null)
				return sb.ToString();

			return string.Empty;
		}

		public byte[] GetBytes()
		{
			using (var stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				var writer = new BinaryWriter(stream);
				WriteTo(writer);
				writer.Flush();
				return stream.ToArray();
			}
		}

		public static string MetadataToCode(MetadataDictionary metadata)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine();
			sb.AppendLine("MetadataDictionary metadata = new MetadataDictionary();");

			foreach (var kvp in metadata._entries)
			{
				int idx = kvp.Key;
				MetadataEntry entry = kvp.Value;

				sb.Append($"metadata[{idx}] = new ");
				switch (entry.Identifier)
				{
					case 0:
					{
						var e = (MetadataByte) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 1:
					{
						var e = (MetadataShort) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 2:
					{
						var e = (MetadataInt) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 3:
					{
						var e = (MetadataFloat) entry;
						sb.Append($"{e.GetType().Name}({e.Value.ToString(NumberFormatInfo.InvariantInfo)}f);");
						break;
					}
					case 4:
					{
						var e = (MetadataString) entry;
						sb.Append($"{e.GetType().Name}(\"{e.Value}\");");
						break;
					}
					case 5:
					{
						var e = (MetadataNbt) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 6:
					{
						var e = (MetadataIntCoordinates) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 7:
					{
						var e = (MetadataLong) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						if (idx == 0)
						{
							sb.Append($" // {Convert.ToString(e.Value, 2)}");
						}
						break;
					}
					case 8:
					{
						var e = (MetadataVector3) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}