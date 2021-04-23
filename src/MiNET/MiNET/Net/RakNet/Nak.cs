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
using System.Collections.Generic;
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public partial class Nak : Packet<Nak>
	{
		public List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();

		public Nak()
		{
			Id = 0xa0;
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			if (Id != 0xa0) throw new Exception("Not NAK");
			ranges.Clear();

			short count = ReadShort(true);
			for (int i = 0; i < count; i++)
			{
				var onlyOneSequence = ReadByte();
				if (onlyOneSequence == 0)
				{
					int start = ReadLittle().IntValue();
					int end = ReadLittle().IntValue();
					if (end - start > 512) end = start + 512;

					var range = new Tuple<int, int>(start, end);
					ranges.Add(range);
				}
				else
				{
					int seqNo = ReadLittle().IntValue();
					var range = new Tuple<int, int>(seqNo, seqNo);
					ranges.Add(range);
				}
			}
		}
		
		/// <inheritdoc />
		protected override void EncodePacket()
		{
			base.EncodePacket();
			
			Write((short) ranges.Count, true);

			
			foreach (var range in ranges)
			{
				if (range.Item1 == range.Item2)
				{
					Write((byte) 1);
					Write(new Int24(range.Item1));
				}
				else
				{
					Write((byte) 0);
					Write(new Int24(range.Item1));
					Write(new Int24(range.Item2));
				}
			}
		}

		/// <inheritdoc />
		protected override void ResetPacket()
		{
			base.ResetPacket();
			ranges.Clear();
		}

		partial void BeforeDecode();
		partial void AfterDecode();
	}
}