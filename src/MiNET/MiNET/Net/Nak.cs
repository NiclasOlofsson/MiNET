using MiNET.Utils;

namespace MiNET.Net
{
	public partial class Nak : Package<Nak>
	{
		public short count; // = null;
		public byte onlyOneSequence; // = null;
		public Int24 sequenceNumber; // = null;

		public Nak()
		{
			Id = 0xa0;
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
}