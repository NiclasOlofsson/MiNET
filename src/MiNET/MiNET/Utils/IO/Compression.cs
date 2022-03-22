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
using System.IO;
using System.IO.Compression;
using log4net;
using MiNET.Net;

namespace MiNET.Utils.IO
{
	public class Compression
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Compression));

		public static byte[] Compress(Memory<byte> input, bool writeLen = false, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				using (var compressStream = new DeflateStream(stream, compressionLevel, true))
				{
					if (writeLen)
					{
						WriteLength(compressStream, input.Length);
					}

					compressStream.Write(input.Span);
				}

				byte[] bytes = stream.ToArray();
				return bytes;
			}
		}

		public static byte[] CompressPacketsForWrapper(List<Packet> packets, CompressionLevel compressionLevel = CompressionLevel.Fastest)
		{
			long length = 0;
			foreach (Packet packet in packets) length += packet.Encode().Length;

			compressionLevel = length > 1000 ? compressionLevel : CompressionLevel.NoCompression;

			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				using (var compressStream = new DeflateStream(stream, compressionLevel, true))
				{
					foreach (Packet packet in packets)
					{
						byte[] bs = packet.Encode();
						if (bs != null && bs.Length > 0)
						{
							BatchUtils.WriteLength(compressStream, bs.Length);
							compressStream.Write(bs, 0, bs.Length);
						}
						packet.PutPool();
					}
					compressStream.Flush();
				}

				byte[] bytes = stream.ToArray();
				return bytes;
			}
		}


		public static void WriteLength(Stream stream, int lenght)
		{
			VarInt.WriteUInt32(stream, (uint) lenght);
		}
	}
}