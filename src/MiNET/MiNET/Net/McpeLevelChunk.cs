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

using System.Collections.Generic;
using log4net;
using MiNET.Blocks;

namespace MiNET.Net
{
	public enum SubChunkRequestMode
	{
		SubChunkRequestModeLegacy,
		SubChunkRequestModeLimitless,
		SubChunkRequestModeLimited
	}
	public partial class McpeLevelChunk : Packet<McpeLevelChunk>
	{
		public ulong[] blobHashes = null;
		public byte[] chunkData = null;
		public bool cacheEnabled;
		//public bool subChunkRequestsEnabled;
		public uint subChunkCount;
		public SubChunkRequestMode subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;

		partial void AfterEncode()
		{
			switch (subChunkRequestMode)
			{
				case SubChunkRequestMode.SubChunkRequestModeLegacy:
				{
					WriteUnsignedVarInt(subChunkCount);

					break;
				}

				case SubChunkRequestMode.SubChunkRequestModeLimitless:
				{
					WriteUnsignedVarInt(uint.MaxValue);
					break;
				}

				case SubChunkRequestMode.SubChunkRequestModeLimited:
				{
					WriteUnsignedVarInt(uint.MaxValue -1);
					Write((ushort)subChunkCount);
					break;
				}
			}

			Write(cacheEnabled);
			
			if (cacheEnabled) 
				Write(blobHashes);
			
			WriteByteArray(chunkData);
		}

		partial void AfterDecode()
		{
			var subChunkCountButNotReally = ReadUnsignedVarInt();

			switch (subChunkCountButNotReally)
			{
				case uint.MaxValue:
					subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLimitless;
					break;
				case uint.MaxValue -1:
					subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLimited;
					subChunkCount = (uint) ReadUshort();
					break;
				default:
					subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;
					subChunkCount = subChunkCountButNotReally;
					break;
			}

			cacheEnabled = ReadBool();
			
			if (cacheEnabled) 
				blobHashes = ReadUlongs();
			
			chunkData = ReadByteArray();
		}
	}

	public partial class McpeClientCacheBlobStatus : Packet<McpeClientCacheBlobStatus>
	{
		public ulong[] hashMisses; // = null;
		public ulong[] hashHits; // = null;

		partial void AfterEncode()
		{
			WriteUnsignedVarInt((uint) hashMisses.Length);
			WriteUnsignedVarInt((uint) hashHits.Length);
			WriteSpecial(hashMisses);
			WriteSpecial(hashHits);
		}

		partial void AfterDecode()
		{
			var lenMisses = ReadUnsignedVarInt();
			var lenHits = ReadUnsignedVarInt();

			hashMisses = ReadUlongsSpecial(lenMisses);
			hashHits = ReadUlongsSpecial(lenHits);
		}

		public void WriteSpecial(ulong[] values)
		{
			if (values == null) return;

			if (values.Length == 0) return;
			for (int i = 0; i < values.Length; i++)
			{
				ulong val = values[i];
				Write(val);
			}
		}

		public ulong[] ReadUlongsSpecial(uint len)
		{
			var values = new ulong[len];
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = ReadUlong();
			}
			return values;
		}
	}

	public partial class McpeClientCacheMissResponse : Packet<McpeClientCacheMissResponse>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(McpeClientCacheMissResponse));

		public Dictionary<ulong, byte[]> blobs;

		partial void AfterEncode()
		{
		}

		partial void AfterDecode()
		{
			blobs = new Dictionary<ulong, byte[]>();
			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				ulong hash = ReadUlong();
				byte[] blob = ReadByteArray();
				if (blobs.ContainsKey(hash))
				{
					Log.Warn($"Already had hash:{hash}. This is most likely air or water");
				}
				else
				{
					blobs.Add(hash, blob);
				}
			}
		}
	}
}