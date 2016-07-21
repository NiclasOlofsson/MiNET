namespace MiNET.Utils
{
	public sealed class PrefixedArray
	{
		private readonly byte[] _array;
		private readonly int _length;

		public PrefixedArray(byte[] array) : this(array, array.Length)
		{
		}

		public PrefixedArray(byte[] array, int length)
		{
			_array = array;
			_length = length;
		}

		public byte[] Array => _array;
		public int Length => _length;
	}
}
