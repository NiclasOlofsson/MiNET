using System;

namespace MiNET.Utils.Skins
{
	public class Cape : ICloneable
	{
		public string Id { get; set; }
		public int ImageHeight { get; set; }
		public int ImageWidth { get; set; }
		public byte[] Data { get; set; }
		public bool OnClassicSkin { get; set; }

		public Cape()
		{
			Data = new byte[0];
		}

		public object Clone() => MemberwiseClone();
	}
}
