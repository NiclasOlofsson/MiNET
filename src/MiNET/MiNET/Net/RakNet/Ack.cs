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
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public class Acks : Packet<Acks>
	{
		public List<int> acks = new List<int>();

		public Acks()
		{
			Id = 0xc0;
		}

		public override void Reset()
		{
			base.Reset();
			acks.Clear();
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			List<Tuple<int, int>> ranges = Slize(acks);

			Write((short) ranges.Count, true);

			foreach (var range in ranges)
			{
				byte singleEntry = (byte) (range.Item1 == range.Item2 ? 0x01 : 0);

				Write(singleEntry);
				Write((Int24) range.Item1);
				if (singleEntry == 0)
					Write((Int24) range.Item2);
			}
		}

		public static List<Tuple<int, int>> Slize(List<int> acks)
		{
			List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();

			if (acks.Count == 0) return ranges;

			int start = acks[0];
			int prev = start;

			if (acks.Count == 1)
			{
				ranges.Add(new Tuple<int, int>(start, start));
				return ranges;
			}

			acks.Sort();

			int i = 0;
			int count = acks.Count;
			int min = start;
			foreach (int ack in acks.ToArray())
			{
				i++;

				bool IsLast = i == count;

				if (start == ack)
				{
					prev = ack;//109
					continue;
				}

				if (prev + 1 == ack)
				{
					if (IsLast)
					{
						ranges.Add(new Tuple<int, int>(min, ack));
					}
				}

				if (prev + 1 != ack)
				{
					if (!IsLast)
					{
						ranges.Add(new Tuple<int, int>(min, prev));
						min = ack;
					}
					else
					{
						ranges.Add(new Tuple<int, int>(min, prev));
						ranges.Add(new Tuple<int, int>(ack, ack));
					}
				}
				prev = ack;
			}
			return ranges;
		}
	}


	public class Ack : Packet<Ack>
	{
		public List<(int, int)> ranges = new List<(int, int)>();

		public Ack()
		{
			Id = 0xc0;
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();

			//if (Id != 0xc0) throw new Exception("Not ACK");

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

					var range = (start, end);
					ranges.Add(range);
				}
				else
				{
					int seqNo = ReadLittle().IntValue();
					var range = (seqNo, seqNo);
					ranges.Add(range);
				}
			}
		}
	}
}
