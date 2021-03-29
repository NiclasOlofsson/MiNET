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
using System.IO;
using System.IO.Compression;
using MiNET.Net;
using MiNET.Net.RakNet;

namespace MiNET.Utils.IO
{
	public class BatchUtils
	{
		public static McpeWrapper CreateBatchPacket(CompressionLevel compressionLevel, params Packet[] packets)
		{
			using (var stream = new MemoryStream())
			{
				foreach (Packet packet in packets)
				{
					byte[] bytes = packet.Encode();
					WriteLength(stream, bytes.Length);
					stream.Write(bytes, 0, bytes.Length);
					packet.PutPool();
				}

				var buffer = new Memory<byte>(stream.GetBuffer(), 0, (int) stream.Length);
				return CreateBatchPacket(buffer, compressionLevel, false);
			}
		}

		public static McpeWrapper CreateBatchPacket(Memory<byte> input, CompressionLevel compressionLevel, bool writeLen)
		{
			var batch = McpeWrapper.CreateObject();
			batch.ReliabilityHeader.Reliability = Reliability.ReliableOrdered;
			batch.payload = Compression.Compress(input, writeLen, input.Length > 1000 ? compressionLevel : CompressionLevel.NoCompression);
			batch.Encode(); // prepare
			return batch;
		}

		public static void WriteLength(Stream stream, int length)
		{
			VarInt.WriteUInt32(stream, (uint) length);
		}
	}
}