namespace MiNET.Utils
{
	public sealed class PrefixedArray
	{
		public PrefixedArray(byte[] array) : this(array, array.Length)
		{
		}

		public PrefixedArray(byte[] array, int length)
		{
			Array = array;
			Length = length;
		}

		public byte[] Array { get; }
		public int Length { get; }
	}
}
