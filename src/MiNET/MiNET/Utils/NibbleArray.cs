using System;

namespace MiNET.Utils
{
	public class NibbleArray: ICloneable
	{
		public byte[] Data { get; set; }

		public NibbleArray()
		{
		}

		public NibbleArray(int length)
		{
			Data = new byte[length/2];
		}

		public int Length
		{
			get { return Data.Length*2; }
		}

		public byte this[int index]
		{
			get { return (byte) (Data[index/2] >> ((index)%2*4) & 0xF); }
			set
			{
				var idx = index >> 1;
				if ((index & 1) == 0)
				{
					Data[idx] |= (byte) (value & 0x0F);
				}
				else
				{
					Data[idx] |= (byte) ((value << 4) & 0xF0);
				}
			}
		}

		public object Clone()
		{
			// BUG sinec this creates a shallow copy, not what we want.
			return MemberwiseClone();
		}
	}
}