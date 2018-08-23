using System;

namespace MiNET.Utils
{
	public class MapInfo: ICloneable
	{
		public long MapId;
		public byte UpdateType;
		public MapDecorator[] Decorators;
		public byte X;
		public byte Z;
		public int Scale;
		public int Col;
		public int Row;
		public int XOffset;
		public int ZOffset;
		public byte[] Data;

		public override string ToString()
		{
			return $"MapId: {MapId}, UpdateType: {UpdateType}, X: {X}, Z: {Z}, Col: {Col}, Row: {Row}, X-offset: {XOffset}, Z-offset: {ZOffset}, Data: {Data?.Length}";
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}

	public class MapDecorator
	{
		public byte Rotation;
		public byte Icon;
		public byte X;
		public byte Z;
		public string Label;
		public uint Color;
	}
}