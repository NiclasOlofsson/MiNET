namespace MiNET.Net
{
	public partial class OpenConnectionRequest1 : Package
	{
		public short mtuSize;

		partial void AfterDecode()
		{
			mtuSize = (short) (((int) (_buffer.Length - _buffer.Position)) + 1);
		}
	}
}
