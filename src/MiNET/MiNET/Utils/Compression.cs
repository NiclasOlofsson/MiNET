using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Utils
{
	class Compression
	{
		public static byte[] Compress(byte[] inputData)
		{
			if (inputData == null)
				throw new ArgumentNullException("inputData");

			using (var compressIntoMs = new MemoryStream())
			{
				using (var gzs = new BufferedStream(new GZipStream(compressIntoMs, CompressionMode.Compress), 2 * 4096))
				{
					gzs.Write(inputData, 0, inputData.Length);
				}
				return compressIntoMs.ToArray();
			}
		}

		public static byte[] Decompress(byte[] inputData)
		{
			if (inputData == null)
				throw new ArgumentNullException("inputData");

			using (var compressedMs = new MemoryStream(inputData))
			{
				using (var decompressedMs = new MemoryStream())
				{
					using (var gzs = new BufferedStream(new GZipStream(compressedMs, CompressionMode.Decompress), 2 * 4096))
					{
						gzs.CopyTo(decompressedMs);
					}
					return decompressedMs.ToArray();
				}
			}
		}
	}
}
