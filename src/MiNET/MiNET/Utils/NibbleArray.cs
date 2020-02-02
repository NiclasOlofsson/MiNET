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

namespace MiNET.Utils
{
	public class NibbleArray : ICloneable
	{
		public byte[] Data { get; set; }

		private NibbleArray()
		{
		}

		public NibbleArray(int length)
		{
			Data = new byte[length / 2];
		}

		public NibbleArray(byte[] data)
		{
			if (data.Length % 2 != 0) throw new ArgumentOutOfRangeException(nameof(data), "Input data must be of size div by 2");

			Data = data;
		}


		public int Length
		{
			get { return Data.Length * 2; }
		}

		public byte this[int index]
		{
			get { return (byte) (Data[index >> 1] >> ((index & 1) * 4) & 0xF); }
			set
			{
				value &= 0xF;
				var idx = index >> 1;
				Data[idx] &= (byte) (0xF << (((index + 1) & 1) * 4));
				Data[idx] |= (byte) (value << ((index & 1) * 4));
			}
		}

		public object Clone()
		{
			NibbleArray nibbleArray = (NibbleArray) MemberwiseClone();
			nibbleArray.Data = (byte[]) Data.Clone();
			return nibbleArray;
		}
	}
}