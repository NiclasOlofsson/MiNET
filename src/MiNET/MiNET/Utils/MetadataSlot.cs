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
using fNbt;
using MiNET.Items;
using MiNET.Net;

namespace MiNET.Utils
{
	public class MetadataSlot : MetadataEntry
	{
		public override byte Identifier
		{
			get { return 5; }
		}

		public override string FriendlyName
		{
			get { return "slot"; }
		}

		public Item Value { get; set; }

		public MetadataSlot()
		{
		}

		public MetadataSlot(Item value)
		{
			Value = value;
		}

		public override void FromStream(BinaryReader reader)
		{
			Value = ReadItem(reader);
		}

		public Item ReadItem(BinaryReader reader)
		{
			int id = ReadSignedVarInt(reader.BaseStream);
			if (id <= 0)
			{
				return new ItemAir();
			}

			int tmp = ReadSignedVarInt(reader.BaseStream);
			short metadata = (short) (tmp >> 8);
			if (metadata == short.MaxValue) metadata = -1;
			byte count = (byte) (tmp & 0xff);
			Item stack = ItemFactory.GetItem((short) id, metadata, count);

			ushort nbtLen = reader.ReadUInt16(); // NbtLen
			if (nbtLen == 0xffff && reader.ReadByte() == 1)
			{
				stack.ExtraData = (NbtCompound) Packet.ReadNbt(reader.BaseStream).NbtFile.RootTag;
			}

			var canPlace = ReadSignedVarInt(reader.BaseStream);
			for (int i = 0; i < canPlace; i++)
			{
				//ReadString();
			}
			var canBreak = ReadSignedVarInt(reader.BaseStream);
			for (int i = 0; i < canBreak; i++)
			{
				//ReadString();
			}

			return stack;
		}

		private int ReadSignedVarInt(Stream buffer)
		{
			return VarInt.ReadSInt32(buffer);
		}


		public override void WriteTo(BinaryWriter stream)
		{
			var stack = Value;

			if (stack == null || stack.Id <= 0)
			{
				WriteSignedVarInt(0, stream.BaseStream);
				return;
			}

			WriteSignedVarInt(stack.Id, stream.BaseStream);
			short metadata = stack.Metadata;
			if (metadata == -1) metadata = short.MaxValue;
			WriteSignedVarInt((metadata << 8) + (stack.Count & 0xff), stream.BaseStream);

			if (stack.ExtraData != null)
			{
				byte[] bytes = Packet.GetNbtData(stack.ExtraData);
				stream.Write((ushort) 0xffff);
				stream.Write((byte) 0x01);
				stream.Write(bytes);
			}
			else
			{
				stream.Write((short) 0);
			}

			WriteSignedVarInt(0, stream.BaseStream);
			WriteSignedVarInt(0, stream.BaseStream);
		}

		private void WriteSignedVarInt(int value, Stream stream)
		{
			VarInt.WriteSInt32(stream, value);
		}
	}
}