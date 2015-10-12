using System;
using System.Collections.Generic;
using MiNET.Utils;

namespace MiNET.Net
{
	public partial class Nak : Package<Nak>
	{
		public List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();

		public Nak()
		{
			Id = 0xa0;
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			ranges.Clear();

			short count = ReadShort();
			for (int i = 0; i < count; i++)
			{
				var onlyOneSequence = ReadByte();
				if (onlyOneSequence == 0)
				{
					int from = ReadLittle().IntValue();
					int to = ReadLittle().IntValue();
					if (to - from > 510) to = from + 512;

					var range = new Tuple<int, int>(from, to);
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

		partial void BeforeDecode();
		partial void AfterDecode();
	}
}