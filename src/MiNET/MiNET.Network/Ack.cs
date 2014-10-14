using System.Collections.Generic;
using MiNET.Network.Utils;

namespace MiNET.Network
{
	public partial class Ack : Package
	{
		public short count; // = null;
		public byte onlyOneSequence; // = null;
		public Int24 sequenceNumber; // = null;

		public Ack()
		{
			Id = 0xc0;
		}

		protected override void EncodePackage()
		{
			base.EncodePackage();

			BeforeEncode();

			Write(count);
			Write(onlyOneSequence);
			Write(sequenceNumber);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePackage()
		{
			base.DecodePackage();

			BeforeDecode();

			count = ReadShort();
			onlyOneSequence = ReadByte();
			sequenceNumber = ReadLittle();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

	}

	//public partial class Ack : Package
	//{
	//	public List<int> nakSequencePackets = new List<int>();
	//	public Int24 sequenceNumber;

	//	partial void AfterEncode()
	//	{
	//		if (onlyOneSequence == 1)
	//		{
	//			Write(sequenceNumber);
	//		}
	//	}

	//	partial void AfterDecode()
	//	{
	//		if (onlyOneSequence != 0)
	//		{
	//			nakSequencePackets.Add(ReadLittle());
	//		}
	//		else
	//		{
	//			for (int i = 0; i < count; i++)
	//			{
	//				int start = ReadLittle();
	//				int end = ReadLittle();
	//				if (start - end > 4096)
	//				{
	//					end = start + 4096;
	//				}
	//				for (int j = start; j <= end; j++)
	//				{
	//					nakSequencePackets.Add(j);
	//				}
	//			}
	//		}
	//	}
	//}

	//public partial class Nak : Package
	//{
	//	public List<int> nakSequencePackets = new List<int>();
	//	private Int24 sequenceNumber;

	//	partial void AfterEncode()
	//	{
	//		if (onlyOneSequence != 0)
	//		{
	//			Write(sequenceNumber);
	//		}
	//	}

	//	partial void AfterDecode()
	//	{
	//		if (onlyOneSequence != 0)
	//		{
	//			nakSequencePackets.Add(ReadLittle());
	//		}
	//		else
	//		{
	//			for (int i = 0; i < count; i++)
	//			{
	//				int start = ReadLittle();
	//				int end = ReadLittle();
	//				if (start - end > 4096)
	//				{
	//					end = start + 4096;
	//				}
	//				for (int j = start; j <= end; j++)
	//				{
	//					nakSequencePackets.Add(j);
	//				}
	//			}
	//		}
	//	}
	//}
}
