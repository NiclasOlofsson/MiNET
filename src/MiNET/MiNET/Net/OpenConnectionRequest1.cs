namespace MiNET.Net
{
	public partial class OpenConnectionRequest1
	{
		public short mtuSize;

		partial void AfterDecode()
		{
			// 1412 but should be 1447
			mtuSize = (short) (((int) (_buffer.Length - _buffer.Position)) + 1/* + 35*/);
		}
	}
}