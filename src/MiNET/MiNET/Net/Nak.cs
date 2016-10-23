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

		partial void BeforeDecode();
		partial void AfterDecode();
	}
}