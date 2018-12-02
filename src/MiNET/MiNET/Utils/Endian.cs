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

namespace MiNET.Utils
{
	public class Endian
	{
		public static short SwapInt16(short v)
		{
			return (short) (((v & 0xff) << 8) | ((v >> 8) & 0xff));
		}

		public static ushort SwapUInt16(ushort v)
		{
			return (ushort) (((v & 0xff) << 8) | ((v >> 8) & 0xff));
		}

		public static int SwapInt32(int v)
		{
			return (int) (((SwapInt16((short) v) & 0xffff) << 0x10) |
						(SwapInt16((short) (v >> 0x10)) & 0xffff));
		}

		public static uint SwapUInt32(uint v)
		{
			return (uint) (((SwapUInt16((ushort) v) & 0xffff) << 0x10) |
							(SwapUInt16((ushort) (v >> 0x10)) & 0xffff));
		}

		public static long SwapInt64(long v)
		{
			return (long) (((SwapInt32((int) v) & 0xffffffffL) << 0x20) |
							(SwapInt32((int) (v >> 0x20)) & 0xffffffffL));
		}

		public static ulong SwapUInt64(ulong v)
		{
			return (ulong) (((SwapUInt32((uint) v) & 0xffffffffL) << 0x20) |
							(SwapUInt32((uint) (v >> 0x20)) & 0xffffffffL));
		}
	}
}