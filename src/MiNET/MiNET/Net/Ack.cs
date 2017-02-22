using System;
using System.Collections.Generic;
using MiNET.Utils;

namespace MiNET.Net
{
	public class Acks : Package<Acks>
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

		protected override void EncodePackage()
		{
			base.EncodePackage();

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


			for (int i = 1; i < acks.Count; i++)
			{
				bool isLast = i + 1 == acks.Count;
				int current = acks[i];

				if (current - prev == 1 && !isLast)
				{
					prev = current;
					continue;
				}

				if (current - prev > 1 && !isLast)
				{
					ranges.Add(new Tuple<int, int>(start, prev));

					start = current;
					prev = current;
					continue;
				}

				if (current - prev == 1 && isLast)
				{
					ranges.Add(new Tuple<int, int>(start, current));
				}

				if (current - prev > 1 && isLast)
				{
					if (prev == start)
					{
						ranges.Add(new Tuple<int, int>(start, current));
					}

					if (prev != start)
					{
						ranges.Add(new Tuple<int, int>(start, prev));
						ranges.Add(new Tuple<int, int>(current, current));
					}
				}
			}

			return ranges;
		}
	}


	public class Ack : Package<Ack>
	{
		public List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();

		public Ack()
		{
			Id = 0xc0;
		}

		protected override void DecodePackage()
		{
			base.DecodePackage();

			if (Id != 0xc0) throw new Exception("Not ACK");

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
	}
}
