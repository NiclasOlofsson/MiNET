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
using System.IO.Compression;

namespace MiNET.Utils
{
	public sealed class ZLibStream : DeflateStream
	{
		private uint _adler = 1;

		private const int ChecksumModulus = 65521;

		public int Checksum => (int) _adler;

		private uint Update(uint adler, byte[] s, int offset, int count)
		{
			uint l = (ushort) adler;
			ulong h = adler >> 16;
			int p = 0;
			for (; p < (count & 7); ++p)
				h += (l += s[offset + p]);

			for (; p < count; p += 8)
			{
				var idx = offset + p;
				h += (l += s[idx]);
				h += (l += s[idx + 1]);
				h += (l += s[idx + 2]);
				h += (l += s[idx + 3]);
				h += (l += s[idx + 4]);
				h += (l += s[idx + 5]);
				h += (l += s[idx + 6]);
				h += (l += s[idx + 7]);
			}

			return (uint) (((h % ChecksumModulus) << 16) | (l % ChecksumModulus));
		}

		public ZLibStream(Stream stream, CompressionLevel level, bool leaveOpen) : base(stream, level, leaveOpen)
		{
		}

		public override void Write(byte[] array, int offset, int count)
		{
			_adler = Update(_adler, array, offset, count);
			base.Write(array, offset, count);
		}
	}
}