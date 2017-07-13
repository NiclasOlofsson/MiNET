namespace MiNET.Utils
{
	public static class BitHelper
	{
		public static void SetBit(ref byte aByte, int pos, bool value)
		{
			if (value)
			{
				aByte = (byte)(aByte | (1 << pos));
			}
			else
			{
				aByte = (byte)(aByte & ~(1 << pos));
			}
		}

		public static bool GetBit(byte aByte, int pos)
		{
			return ((aByte & (1 << pos)) != 0);
		}
	}
}
