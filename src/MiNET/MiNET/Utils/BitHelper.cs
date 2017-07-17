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

		public static byte SetBit(byte aByte, int pos, bool value)
		{
			if (value)
			{
				return (byte)(aByte | (1 << pos));
			}
			else
			{
				return (byte)(aByte & ~(1 << pos));
			}
		}

		public static byte ToggleBit(byte aByte, int pos)
		{
			return (byte)(aByte ^ (pos));
		}

		public static bool GetBit(byte aByte, int pos)
		{
			return (aByte & pos) != 0;
		}
	}
}
