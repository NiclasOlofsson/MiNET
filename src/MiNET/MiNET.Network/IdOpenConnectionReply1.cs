namespace MiNET
{
	public partial class IdOpenConnectionRequest1 : Package
	{
		public short mtuSize;

		partial void AfterDecode()
		{
			mtuSize = (short) (((int) (_buffer.Length - _buffer.Position)) + 1);
		}
	}
}
