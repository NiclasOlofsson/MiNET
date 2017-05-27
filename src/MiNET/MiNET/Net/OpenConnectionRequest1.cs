namespace MiNET.Net
{
	public partial class OpenConnectionRequest1
	{
		public short mtuSize;

		partial void AfterDecode()
		{
			//mtuSize = (short) (((int) (_buffer.Length - _buffer.Position)) + 18);
			//mtuSize = (short) (_buffer.Length + 8 + 20);
			mtuSize = (short) (_buffer.Length);
			ReadBytes((int) (_buffer.Length - 18));
		}
	}
}